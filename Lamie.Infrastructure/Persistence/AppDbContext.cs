using Lamie.Domain.Entities;
using Lamie.Domain.Entities.Auth;
using Lamie.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;

namespace Lamie.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductTranslation> ProductTranslations => Set<ProductTranslation>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();

    public DbSet<ProductCollection> ProductCollections => Set<ProductCollection>();
    public DbSet<ProductColor> ProductColors => Set<ProductColor>();
    public DbSet<ProductTag> ProductTags => Set<ProductTag>();
    public DbSet<ProductStyle> ProductStyles => Set<ProductStyle>();
    public DbSet<ProductOccasion> ProductOccasions => Set<ProductOccasion>();

    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<TagTranslation> TagTranslations => Set<TagTranslation>();

    public DbSet<Color> Colors => Set<Color>();
    public DbSet<ColorTranslation> ColorTranslations => Set<ColorTranslation>();

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<CategoryTranslation> CategoryTranslations => Set<CategoryTranslation>();

    public DbSet<Collection> Collections => Set<Collection>();
    public DbSet<CollectionTranslation> CollectionTranslations => Set<CollectionTranslation>();

    public DbSet<Occasion> Occasions => Set<Occasion>();
    public DbSet<OccasionTranslation> OccasionTranslations => Set<OccasionTranslation>();

    public DbSet<Style> Styles => Set<Style>();
    public DbSet<StyleTranslation> StyleTranslations => Set<StyleTranslation>();

    public DbSet<Language> Languages => Set<Language>();

    public DbSet<Channel> Channels => Set<Channel>();

    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<OrderImage> OrderImages => Set<OrderImage>();
    public DbSet<OrderChangeLog> OrderChangeLogs => Set<OrderChangeLog>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
