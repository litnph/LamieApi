using Lamie.Application.Common.Auth;
using Lamie.Application.Common.Storage;
using Lamie.Domain.Repositories;
using Lamie.Infrastructure.Auth;
using Lamie.Infrastructure.Hosting;
using Lamie.Infrastructure.Options;
using Lamie.Infrastructure.Persistence.Repositories;
using Lamie.Infrastructure.Storage;
using Lamie.Shared.Auth;
using Lamie.Shared.Time;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
}
