using Lamie.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lamie.Infrastructure.Persistence.Configurations.Catalog;

public sealed class ProductTranslationConfiguration : IEntityTypeConfiguration<ProductTranslation>
{
    public void Configure(EntityTypeBuilder<ProductTranslation> builder)
    {
        builder.ToTable("cat_product_translations");
        builder.ConfigureEntityBase();

        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.LanguageCode).IsRequired().HasMaxLength(10);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Slug).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Description).HasColumnType("text");

        builder.HasIndex(x => new { x.ProductId, x.LanguageCode }).IsUnique();
    }
}
