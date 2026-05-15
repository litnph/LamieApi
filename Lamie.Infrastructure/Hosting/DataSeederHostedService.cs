using Lamie.Application.Common.Auth;
using Lamie.Domain.Entities;
using Lamie.Domain.Entities.Auth;
using Lamie.Domain.Repositories;
using Lamie.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Lamie.Infrastructure.Hosting;

/// <summary>
/// Idempotent startup seeder. Runs once on app start.
/// - Ensures supported Languages (vi, en) exist.
/// - Ensures default Channels (META, TIKTOK, ZALO) exist.
/// - Creates the bootstrap admin user when no users are present.
/// </summary>
public sealed class DataSeederHostedService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DataSeederHostedService> _logger;

    public DataSeederHostedService(
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration,
        ILogger<DataSeederHostedService> logger)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var sp = scope.ServiceProvider;
        var db = sp.GetRequiredService<AppDbContext>();

        await AlignEfMigrationsHistoryToSnakeCaseAsync(db, cancellationToken);
        await db.Database.MigrateAsync(cancellationToken);

        await SeedLanguagesAsync(db, cancellationToken);
        await SeedChannelsAsync(db, cancellationToken);
        await SeedAdminAsync(sp, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    // Legacy DBs may have PascalCase columns on __EFMigrationsHistory; NamingConventions expects snake_case, which blocks Migrate.
    private static async Task AlignEfMigrationsHistoryToSnakeCaseAsync(
        AppDbContext db,
        CancellationToken cancellationToken)
    {
        const string sql = """
            DO $efnorm$
            BEGIN
              IF EXISTS (
                SELECT 1
                FROM pg_catalog.pg_attribute a
                INNER JOIN pg_catalog.pg_class c ON c.oid = a.attrelid
                INNER JOIN pg_catalog.pg_namespace n ON n.oid = c.relnamespace
                WHERE n.nspname = 'public'
                  AND c.relname = '__EFMigrationsHistory'
                  AND a.attnum > 0
                  AND NOT a.attisdropped
                  AND a.attname = 'MigrationId'
              ) THEN
                ALTER TABLE public."__EFMigrationsHistory" RENAME COLUMN "MigrationId" TO migration_id;
              END IF;

              IF EXISTS (
                SELECT 1
                FROM pg_catalog.pg_attribute a
                INNER JOIN pg_catalog.pg_class c ON c.oid = a.attrelid
                INNER JOIN pg_catalog.pg_namespace n ON n.oid = c.relnamespace
                WHERE n.nspname = 'public'
                  AND c.relname = '__EFMigrationsHistory'
                  AND a.attnum > 0
                  AND NOT a.attisdropped
                  AND a.attname = 'ProductVersion'
              ) THEN
                ALTER TABLE public."__EFMigrationsHistory" RENAME COLUMN "ProductVersion" TO product_version;
              END IF;
            END $efnorm$;
            """;

        await db.Database.ExecuteSqlRawAsync(sql, cancellationToken);
    }

    private async Task SeedLanguagesAsync(AppDbContext db, CancellationToken cancellationToken)
    {
        var defaults = new[]
        {
            new Language("vi", "Tiếng Việt"),
            new Language("en", "English"),
        };

        foreach (var lang in defaults)
        {
            var exists = await db.Languages.AnyAsync(l => l.Code == lang.Code, cancellationToken);
            if (!exists)
            {
                db.Languages.Add(lang);
            }
        }

        await db.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedChannelsAsync(AppDbContext db, CancellationToken cancellationToken)
    {
        var defaults = new[]
        {
            new Channel("META", "Meta Business", null, 1),
            new Channel("TIKTOK", "TikTok", null, 2),
            new Channel("ZALO", "Zalo", null, 3),
        };

        foreach (var channel in defaults)
        {
            var exists = await db.Channels.AnyAsync(c => c.Code == channel.Code, cancellationToken);
            if (!exists)
            {
                db.Channels.Add(channel);
            }
        }

        await db.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedAdminAsync(IServiceProvider sp, CancellationToken cancellationToken)
    {
        var userRepository = sp.GetRequiredService<IUserRepository>();
        if (await userRepository.AnyAsync(cancellationToken)) return;

        var hasher = sp.GetRequiredService<IPasswordHasher>();

        var section = _configuration.GetSection("Seed:Admin");
        var email = section["Email"];
        var userName = section["UserName"];
        var password = section["Password"];
        var fullName = section["FullName"];

        if (string.IsNullOrWhiteSpace(email)
            || string.IsNullOrWhiteSpace(userName)
            || string.IsNullOrWhiteSpace(password)
            || string.IsNullOrWhiteSpace(fullName))
        {
            _logger.LogWarning(
                "Seed:Admin section is incomplete; bootstrap admin user was not created. Provide Email, UserName, Password and FullName.");
            return;
        }

        var admin = new User(email, userName, hasher.Hash(password), fullName, UserRole.Admin);
        await userRepository.AddAsync(admin, cancellationToken);
        _logger.LogInformation("Seeded admin user '{UserName}'.", userName);
    }
}
