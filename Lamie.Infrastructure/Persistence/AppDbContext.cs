using Lamie.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Lamie.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductTranslation> ProductTranslations => Set<ProductTranslation>();
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();

        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<TagTranslation> TagTranslations => Set<TagTranslation>();

        public DbSet<Language> Languages => Set<Language>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("cat_products");
                entity.HasKey(x => x.Id);
            });

            modelBuilder.Entity<ProductTranslation>(entity =>
            {
                entity.ToTable("cat_product_translations");
                entity.HasKey(x => x.Id);
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.ToTable("cat_product_images");
                entity.HasKey(x => x.Id);
            });

            // Tags
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("md_tags");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.IsActive)
                      .HasColumnName("is_active");

                // Map backing field _translations
                entity.Metadata.FindNavigation(nameof(Tag.Translations))!
                    .SetPropertyAccessMode(PropertyAccessMode.Field);

                entity.HasMany(t => t.Translations)
                      .WithOne()
                      .HasForeignKey(t => t.TagId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TagTranslation>(entity =>
            {
                entity.ToTable("md_tag_translations");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.TagId)
                      .HasColumnName("tag_id");

                entity.Property(x => x.LanguageCode)
                      .HasColumnName("language_code");

                entity.Property(x => x.Name)
                      .HasColumnName("name");

                entity.Property(x => x.Description)
                      .HasColumnName("description");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.ToTable("sys_languages");
                entity.HasKey(x => x.Code);

                entity.Property(x => x.Code)
                      .HasColumnName("code");

                entity.Property(x => x.Name)
                      .HasColumnName("name");

                entity.Property(x => x.IsActive)
                      .HasColumnName("is_active");
            });
        }
        public override int SaveChanges()
        {
            ApplyAudit();
            return base.SaveChanges();
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAudit();
            return await base.SaveChangesAsync(cancellationToken);
        }
        private void ApplyAudit()
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<Entity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = now;
                        entry.Entity.CreatedBy = -99;
                        entry.Entity.CreatedName = "dev";
                        entry.Entity.UpdatedAt = now;
                        entry.Entity.UpdatedBy = -99;
                        entry.Entity.UpdatedName = "dev";
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = now;
                        entry.Entity.UpdatedBy = -100;
                        entry.Entity.UpdatedName = "test";
                        break;
                }
            }
        }
    }
}
