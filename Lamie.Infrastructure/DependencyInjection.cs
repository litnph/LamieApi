using Lamie.Application.Common.Auth;
using Lamie.Application.Common.Storage;
using Lamie.Domain.Repositories;
using Lamie.Infrastructure.Auth;
using Lamie.Infrastructure.Hosting;
using Lamie.Infrastructure.Options;
using Lamie.Infrastructure.Persistence;
using Lamie.Infrastructure.Persistence.Interceptors;
using Lamie.Infrastructure.Persistence.Repositories;
using Lamie.Infrastructure.Storage;
using Lamie.Shared.Auth;
using Lamie.Shared.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Lamie.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SupabaseOptions>(configuration.GetSection(SupabaseOptions.SectionName));
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

        services.AddSingleton<IClock, SystemClock>();
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, HttpContextUserContext>();

        services.AddScoped<AuditInterceptor>();
        services.AddScoped<OrderAuditInterceptor>();

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            var connectionString = ResolvePostgreSqlConnectionString(configuration);
            options.UseNpgsql(
                connectionString,
                npg => npg.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name));

            options.UseSnakeCaseNamingConvention();

            options.AddInterceptors(
                sp.GetRequiredService<AuditInterceptor>(),
                sp.GetRequiredService<OrderAuditInterceptor>());
        });

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IColorRepository, ColorRepository>();
        services.AddScoped<ICollectionRepository, CollectionRepository>();
        services.AddScoped<IOccasionRepository, OccasionRepository>();
        services.AddScoped<IStyleRepository, StyleRepository>();
        services.AddScoped<ILanguageRepository, LanguageRepository>();
        services.AddScoped<IChannelRepository, ChannelRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();

        services.AddHttpClient<IFileStorage, SupabaseFileStorage>();

        services.AddHostedService<DataSeederHostedService>();

        return services;
    }

    /// <summary>
    /// Uses <c>ConnectionStrings:Default</c> when non-empty; otherwise common database URL
    /// environment variables (see <see cref="GetDatabaseUrlFromEnvironment"/>).
    /// </summary>
    private static string ResolvePostgreSqlConnectionString(IConfiguration configuration)
    {
        var raw = configuration.GetConnectionString("Default");
        if (string.IsNullOrWhiteSpace(raw))
            raw = GetDatabaseUrlFromEnvironment(configuration);

        if (string.IsNullOrWhiteSpace(raw))
        {
            throw new InvalidOperationException(
                "PostgreSQL is not configured. On your Render Web Service (the service that runs this API), add an environment variable: " +
                "either DATABASE_URL, or ConnectionStrings__Default, with the value from your Render Postgres → Info → Internal Database URL " +
                "(postgresql://...). Render does not inject DATABASE_URL automatically unless you define it in render.yaml (fromDatabase) or add it manually.");
        }

        try
        {
            return ToNpgsqlConnectionString(raw.Trim());
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "The PostgreSQL connection string or URI could not be parsed for Npgsql.", ex);
        }
    }

    /// <summary>
    /// ADO.NET treats ';' and '=' specially; a <c>postgresql://...?sslmode=require&amp;...</c> URI must not be passed
    /// verbatim to <see cref="NpgsqlConnection"/> — convert to an Npgsql key/value connection string.
    /// </summary>
    private static string ToNpgsqlConnectionString(string raw)
    {
        if (!raw.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase)
            && !raw.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
            return new NpgsqlConnectionStringBuilder(raw).ConnectionString;

        var uri = new Uri(raw);
        var userInfo = uri.UserInfo.Split(':', 2);
        var username = Uri.UnescapeDataString(userInfo[0]);
        var password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : string.Empty;

        var path = uri.AbsolutePath.TrimStart('/');
        var dbQuerySplit = path.IndexOf('?', StringComparison.Ordinal);
        var database = dbQuerySplit >= 0 ? path[..dbQuerySplit] : path;

        var query = ParsePostgresUriQuery(uri.Query);

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.Port > 0 ? uri.Port : 5432,
            Username = username,
            Password = password,
            Database = database,
        };

        if (query.TryGetValue("sslmode", out var sslMode))
            builder.SslMode = ParseSslMode(sslMode);
        else
            builder.SslMode = SslMode.Require;

        if (query.TryGetValue("channel_binding", out var channelBinding)
            && channelBinding.Equals("require", StringComparison.OrdinalIgnoreCase))
            builder.ChannelBinding = ChannelBinding.Require;

        return builder.ConnectionString;
    }

    private static Dictionary<string, string> ParsePostgresUriQuery(string query)
    {
        var d = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        if (string.IsNullOrEmpty(query) || query == "?")
            return d;

        var trimmed = query.StartsWith("?", StringComparison.Ordinal) ? query[1..] : query;
        foreach (var segment in trimmed.Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var eq = segment.IndexOf('=');
            if (eq <= 0)
                continue;

            d[Uri.UnescapeDataString(segment[..eq])] = Uri.UnescapeDataString(segment[(eq + 1)..]);
        }

        return d;
    }

    private static SslMode ParseSslMode(string value) =>
        value.ToLowerInvariant() switch
        {
            "disable" => SslMode.Disable,
            "allow" => SslMode.Allow,
            "prefer" => SslMode.Prefer,
            "require" => SslMode.Require,
            "verify-ca" or "verifyca" => SslMode.VerifyCA,
            "verify-full" or "verifyfull" => SslMode.VerifyFull,
            _ => SslMode.Require,
        };

    /// <summary>
    /// Reads common env var names from <see cref="IConfiguration"/> and from the process environment.
    /// </summary>
    private static string? GetDatabaseUrlFromEnvironment(IConfiguration configuration)
    {
        string[] keys =
        [
            "DATABASE_URL",
            "POSTGRES_URL",
            "POSTGRESQL_URL",
            "POSTGRES_PRISMA_URL",
        ];

        foreach (var key in keys)
        {
            var value = configuration[key];
            if (!string.IsNullOrWhiteSpace(value))
                return value;

            value = Environment.GetEnvironmentVariable(key);
            if (!string.IsNullOrWhiteSpace(value))
                return value;
        }

        return null;
    }
}
