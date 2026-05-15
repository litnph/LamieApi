using Lamie.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lamie.Infrastructure.Persistence.Configurations.MasterData;

public sealed class StyleConfiguration : IEntityTypeConfiguration<Style>
{
    public void Configure(EntityTypeBuilder<Style> builder)
    {
        builder.ToTable("md_styles");
        builder.ConfigureEntityBase();

        builder.Property(x => x.IsActive).IsRequired();

        builder.Metadata.FindNavigation(nameof(Style.Translations))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(c => c.Translations)
            .WithOne()
            .HasForeignKey(t => t.StyleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class StyleTranslationConfiguration : IEntityTypeConfiguration<StyleTranslation>
{
    public void Configure(EntityTypeBuilder<StyleTranslation> builder)
    {
        builder.ToTable("md_style_translations");
        builder.ConfigureEntityBase();
        builder.Property(x => x.StyleId).IsRequired();
        builder.Property(x => x.LanguageCode).IsRequired().HasMaxLength(10);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Description).HasColumnType("text");

        builder.HasIndex(x => new { x.StyleId, x.LanguageCode }).IsUnique();
    }
}
