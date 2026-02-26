using Lamie.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lamie.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<SysUser> SysUsers => Set<SysUser>();
        public DbSet<Product> Products => Set<Product>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("cat_products");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Sku)
                      .HasColumnName("sku");

                entity.Property(x => x.Price)
                      .HasColumnName("price");

                entity.Property(x => x.SalePrice)
                      .HasColumnName("sale_price");

                entity.Property(x => x.Stock)
                      .HasColumnName("stock");

                entity.Property(x => x.IsActive)
                      .HasColumnName("is_active");
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

            //base.OnModelCreating(modelBuilder);
        }

    }
}
