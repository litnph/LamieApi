using Lamie.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lamie.Infrastructure.Persistence.Configurations.Catalog;

public sealed class ProductCollectionConfiguration : IEntityTypeConfiguration<ProductCollection>
{
    public void Configure(EntityTypeBuilder<ProductCollection> builder)
    {
        builder.ToTable("rel_product_collections");
        builder.ConfigureEntityBase();
        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.CollectionId).IsRequired();
        builder.HasIndex(x => new { x.ProductId, x.CollectionId }).IsUnique();
    }
}

public sealed class ProductColorConfiguration : IEntityTypeConfiguration<ProductColor>
{
    public void Configure(EntityTypeBuilder<ProductColor> builder)
    {
        builder.ToTable("rel_product_colors");
        builder.ConfigureEntityBase();
        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.ColorId).IsRequired();
        builder.HasIndex(x => new { x.ProductId, x.ColorId }).IsUnique();
    }
}

public sealed class ProductTagConfiguration : IEntityTypeConfiguration<ProductTag>
{
    public void Configure(EntityTypeBuilder<ProductTag> builder)
    {
        builder.ToTable("rel_product_tags");
        builder.ConfigureEntityBase();
        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.TagId).IsRequired();
        builder.HasIndex(x => new { x.ProductId, x.TagId }).IsUnique();
    }
}

public sealed class ProductStyleConfiguration : IEntityTypeConfiguration<ProductStyle>
{
    public void Configure(EntityTypeBuilder<ProductStyle> builder)
    {
        builder.ToTable("rel_product_styles");
        builder.ConfigureEntityBase();
        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.StyleId).IsRequired();
        builder.HasIndex(x => new { x.ProductId, x.StyleId }).IsUnique();
    }
}

public sealed class ProductOccasionConfiguration : IEntityTypeConfiguration<ProductOccasion>
{
    public void Configure(EntityTypeBuilder<ProductOccasion> builder)
    {
        builder.ToTable("rel_product_occasions");
        builder.ConfigureEntityBase();
        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.OccasionId).IsRequired();
        builder.HasIndex(x => new { x.ProductId, x.OccasionId }).IsUnique();
    }
}
