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

                entity.Property(x => x.Sku).HasColumnName("sku");
                entity.Property(x => x.Price).HasColumnName("price");
                entity.Property(x => x.SalePrice).HasColumnName("sale_price");
                entity.Property(x => x.Stock).HasColumnName("stock");
                entity.Property(x => x.CategoryId).HasColumnName("category_id");
                entity.Property(x => x.IsActive).HasColumnName("is_active");

                entity.Metadata.FindNavigation(nameof(Product.Translations))!
                    .SetPropertyAccessMode(PropertyAccessMode.Field);
                entity.Metadata.FindNavigation(nameof(Product.Images))!
                    .SetPropertyAccessMode(PropertyAccessMode.Field);
                entity.Metadata.FindNavigation(nameof(Product.Collections))!
                    .SetPropertyAccessMode(PropertyAccessMode.Field);
                entity.Metadata.FindNavigation(nameof(Product.Colors))!
                    .SetPropertyAccessMode(PropertyAccessMode.Field);
                entity.Metadata.FindNavigation(nameof(Product.Tags))!
                    .SetPropertyAccessMode(PropertyAccessMode.Field);
                entity.Metadata.FindNavigation(nameof(Product.Styles))!
                    .SetPropertyAccessMode(PropertyAccessMode.Field);
                entity.Metadata.FindNavigation(nameof(Product.Occasions))!
                    .SetPropertyAccessMode(PropertyAccessMode.Field);

                entity.HasMany(p => p.Translations)
                      .WithOne()
                      .HasForeignKey(t => t.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.Images)
                      .WithOne()
                      .HasForeignKey(i => i.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.Collections)
                      .WithOne()
                      .HasForeignKey(r => r.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.Colors)
                      .WithOne()
                      .HasForeignKey(r => r.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.Tags)
                      .WithOne()
                      .HasForeignKey(r => r.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.Styles)
                      .WithOne()
                      .HasForeignKey(r => r.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.Occasions)
                      .WithOne()
                      .HasForeignKey(r => r.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ProductTranslation>(entity =>
            {
                entity.ToTable("cat_product_translations");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.ProductId).HasColumnName("product_id");
                entity.Property(x => x.LanguageCode).HasColumnName("language_code");
                entity.Property(x => x.Name).HasColumnName("name");
                entity.Property(x => x.Slug).HasColumnName("slug");
                entity.Property(x => x.Description).HasColumnName("description");
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.ToTable("cat_product_images");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.ProductId).HasColumnName("product_id");
                entity.Property(x => x.ImageUrl).HasColumnName("image_url");
                entity.Property(x => x.IsActive).HasColumnName("is_active");
                entity.Property(x => x.SortOrder).HasColumnName("sort_order");
            });

            // Product relationships
            modelBuilder.Entity<ProductCollection>(entity =>
            {
                entity.ToTable("rel_product_collections");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.ProductId).HasColumnName("product_id");
                entity.Property(x => x.CollectionId).HasColumnName("collection_id");
            });

            modelBuilder.Entity<ProductColor>(entity =>
            {
                entity.ToTable("rel_product_colors");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.ProductId).HasColumnName("product_id");
                entity.Property(x => x.ColorId).HasColumnName("color_id");
            });

            modelBuilder.Entity<ProductTag>(entity =>
            {
                entity.ToTable("rel_product_tags");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.ProductId).HasColumnName("product_id");
                entity.Property(x => x.TagId).HasColumnName("tag_id");
            });

            modelBuilder.Entity<ProductStyle>(entity =>
            {
                entity.ToTable("rel_product_styles");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.ProductId).HasColumnName("product_id");
                entity.Property(x => x.StyleId).HasColumnName("style_id");
            });

            modelBuilder.Entity<ProductOccasion>(entity =>
            {
                entity.ToTable("rel_product_occasions");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.ProductId).HasColumnName("product_id");
                entity.Property(x => x.OccasionId).HasColumnName("occasion_id");
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

            // Colors
            modelBuilder.Entity<Color>(entity =>
            {
                entity.ToTable("md_colors");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.HexCode)
                      .HasColumnName("hex_code");

                entity.Property(x => x.RgbCode)
                      .HasColumnName("rgb_code");

                entity.Property(x => x.IsActive)
                      .HasColumnName("is_active");

                entity.Metadata.FindNavigation(nameof(Color.Translations))!
                    .SetPropertyAccessMode(PropertyAccessMode.Field);

                entity.HasMany(c => c.Translations)
                      .WithOne()
                      .HasForeignKey(t => t.ColorId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ColorTranslation>(entity =>
            {
                entity.ToTable("md_color_translations");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.ColorId)
                      .HasColumnName("color_id");

                entity.Property(x => x.LanguageCode)
                      .HasColumnName("language_code");

                entity.Property(x => x.Name)
                      .HasColumnName("name");

                entity.Property(x => x.Description)
                      .HasColumnName("description");
            });

            // Categories
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("md_categories");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.SortOrder)
                      .HasColumnName("sort_order");

                entity.Property(x => x.IsActive)
                      .HasColumnName("is_active");

                entity.Metadata.FindNavigation(nameof(Category.Translations))!
                    .SetPropertyAccessMode(PropertyAccessMode.Field);

                entity.HasMany(c => c.Translations)
                      .WithOne()
                      .HasForeignKey(t => t.CategoryId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CategoryTranslation>(entity =>
            {
                entity.ToTable("md_category_translations");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.CategoryId)
                      .HasColumnName("category_id");

                entity.Property(x => x.LanguageCode)
                      .HasColumnName("language_code");

                entity.Property(x => x.Name)
                      .HasColumnName("name");

                entity.Property(x => x.Description)
                      .HasColumnName("description");
            });

            // Collections
            modelBuilder.Entity<Collection>(entity =>
            {
                entity.ToTable("md_collections");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.IsActive)
                      .HasColumnName("is_active");

                entity.Metadata.FindNavigation(nameof(Collection.Translations))!
                    .SetPropertyAccessMode(PropertyAccessMode.Field);

                entity.HasMany(c => c.Translations)
                      .WithOne()
                      .HasForeignKey(t => t.CollectionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CollectionTranslation>(entity =>
            {
                entity.ToTable("md_collection_translations");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.CollectionId)
                      .HasColumnName("collection_id");

                entity.Property(x => x.LanguageCode)
                      .HasColumnName("language_code");

                entity.Property(x => x.Name)
                      .HasColumnName("name");

                entity.Property(x => x.Description)
                      .HasColumnName("description");
            });

            // Occasions
            modelBuilder.Entity<Occasion>(entity =>
            {
                entity.ToTable("md_occasions");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.IsActive)
                      .HasColumnName("is_active");

                entity.Metadata.FindNavigation(nameof(Occasion.Translations))!
                    .SetPropertyAccessMode(PropertyAccessMode.Field);

                entity.HasMany(o => o.Translations)
                      .WithOne()
                      .HasForeignKey(t => t.OccasionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OccasionTranslation>(entity =>
            {
                entity.ToTable("md_occasion_translations");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.OccasionId)
                      .HasColumnName("occasion_id");

                entity.Property(x => x.LanguageCode)
                      .HasColumnName("language_code");

                entity.Property(x => x.Name)
                      .HasColumnName("name");

                entity.Property(x => x.Description)
                      .HasColumnName("description");
            });

            // Styles
            modelBuilder.Entity<Style>(entity =>
            {
                entity.ToTable("md_styles");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.IsActive)
                      .HasColumnName("is_active");

                entity.Metadata.FindNavigation(nameof(Style.Translations))!
                    .SetPropertyAccessMode(PropertyAccessMode.Field);

                entity.HasMany(s => s.Translations)
                      .WithOne()
                      .HasForeignKey(t => t.StyleId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<StyleTranslation>(entity =>
            {
                entity.ToTable("md_style_translations");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.StyleId)
                      .HasColumnName("style_id");

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
