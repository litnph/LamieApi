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
    /// Uses <c>ConnectionStrings:Default</c> when non-empty; otherwise <c>DATABASE_URL</c>
    /// (Render and other hosts inject this when PostgreSQL is linked).
    /// </summary>
    private static string ResolvePostgreSqlConnectionString(IConfiguration configuration)
    {
        var fromConfig = configuration.GetConnectionString("Default");
        if (!string.IsNullOrWhiteSpace(fromConfig))
            return fromConfig;

        var databaseUrl = configuration["DATABASE_URL"];
        if (string.IsNullOrWhiteSpace(databaseUrl))
        {
            throw new InvalidOperationException(
                "PostgreSQL is not configured. Set ConnectionStrings__Default to your connection string, " +
                "or link a Render PostgreSQL instance so DATABASE_URL is injected.");
        }

        try
        {
            return new NpgsqlConnectionStringBuilder(databaseUrl.Trim()).ConnectionString;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "DATABASE_URL is set but could not be parsed as a PostgreSQL connection string or URI.", ex);
        }
    }
}
