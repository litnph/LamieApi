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
        var fromConfig = configuration.GetConnectionString("Default");
        if (!string.IsNullOrWhiteSpace(fromConfig))
            return fromConfig;

        var databaseUrl = GetDatabaseUrlFromEnvironment(configuration);
        if (string.IsNullOrWhiteSpace(databaseUrl))
        {
            throw new InvalidOperationException(
                "PostgreSQL is not configured. On your Render Web Service (the service that runs this API), add an environment variable: " +
                "either DATABASE_URL, or ConnectionStrings__Default, with the value from your Render Postgres → Info → Internal Database URL " +
                "(postgresql://...). Render does not inject DATABASE_URL automatically unless you define it in render.yaml (fromDatabase) or add it manually.");
        }

        try
        {
            return new NpgsqlConnectionStringBuilder(databaseUrl.Trim()).ConnectionString;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "The database URL environment variable is set but could not be parsed as a PostgreSQL connection string or URI.", ex);
        }
    }

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
