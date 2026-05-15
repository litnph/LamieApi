using Lamie.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lamie.Infrastructure.Persistence.Configurations.MasterData;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("md_categories");
        builder.ConfigureEntityBase();

        builder.Property(x => x.SortOrder).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();

        builder.Metadata.FindNavigation(nameof(Category.Translations))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(c => c.Translations)
            .WithOne()
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class CategoryTranslationConfiguration : IEntityTypeConfiguration<CategoryTranslation>
{
    public void Configure(EntityTypeBuilder<CategoryTranslation> builder)
    {
        builder.ToTable("md_category_translations");
        builder.ConfigureEntityBase();
        builder.Property(x => x.CategoryId).IsRequired();
        builder.Property(x => x.LanguageCode).IsRequired().HasMaxLength(10);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Description).HasColumnType("text");

        builder.HasIndex(x => new { x.CategoryId, x.LanguageCode }).IsUnique();
    }
}
