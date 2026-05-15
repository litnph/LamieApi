using Lamie.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lamie.Infrastructure.Persistence.Configurations.Catalog;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("cat_products");
        builder.ConfigureEntityBase();

        builder.Property(p => p.Sku).IsRequired().HasMaxLength(100);
        builder.HasIndex(p => p.Sku).IsUnique();

        builder.Property(p => p.Price).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(p => p.SalePrice).HasColumnType("numeric(18,2)");
        builder.Property(p => p.Stock).IsRequired();
        builder.Property(p => p.CategoryId).IsRequired();
        builder.Property(p => p.IsActive).IsRequired();
        builder.Property(p => p.ThumbnailUrl).HasMaxLength(1000);

        builder.HasIndex(p => p.CategoryId);
        builder.HasIndex(p => p.IsActive);

        SetBackingField(builder, nameof(Product.Translations));
        SetBackingField(builder, nameof(Product.Images));
        SetBackingField(builder, nameof(Product.Collections));
        SetBackingField(builder, nameof(Product.Colors));
        SetBackingField(builder, nameof(Product.Tags));
        SetBackingField(builder, nameof(Product.Styles));
        SetBackingField(builder, nameof(Product.Occasions));

        builder.HasMany(p => p.Translations)
            .WithOne()
            .HasForeignKey(t => t.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Images)
            .WithOne()
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Collections)
            .WithOne()
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Colors)
            .WithOne()
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Tags)
            .WithOne()
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Styles)
            .WithOne()
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Occasions)
            .WithOne()
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void SetBackingField(EntityTypeBuilder<Product> builder, string nav)
    {
        builder.Metadata.FindNavigation(nav)!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
