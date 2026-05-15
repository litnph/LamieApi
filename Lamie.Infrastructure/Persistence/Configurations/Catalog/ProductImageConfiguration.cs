using Lamie.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lamie.Infrastructure.Persistence.Configurations.Catalog;

public sealed class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable("cat_product_images");
        builder.ConfigureEntityBase();

        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.ImageUrl).IsRequired().HasMaxLength(1000);
        builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(x => x.SortOrder).IsRequired();

        builder.HasIndex(x => x.ProductId);
    }
}
