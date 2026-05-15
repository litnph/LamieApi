using Lamie.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lamie.Infrastructure.Persistence.Configurations.MasterData;

public sealed class ColorConfiguration : IEntityTypeConfiguration<Color>
{
    public void Configure(EntityTypeBuilder<Color> builder)
    {
        builder.ToTable("md_colors");
        builder.ConfigureEntityBase();

        builder.Property(x => x.HexCode).IsRequired().HasMaxLength(20);
        builder.Property(x => x.RgbCode).IsRequired().HasMaxLength(30);
        builder.Property(x => x.IsActive).IsRequired();

        builder.Metadata.FindNavigation(nameof(Color.Translations))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(c => c.Translations)
            .WithOne()
            .HasForeignKey(t => t.ColorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class ColorTranslationConfiguration : IEntityTypeConfiguration<ColorTranslation>
{
    public void Configure(EntityTypeBuilder<ColorTranslation> builder)
    {
        builder.ToTable("md_color_translations");
        builder.ConfigureEntityBase();
        builder.Property(x => x.ColorId).IsRequired();
        builder.Property(x => x.LanguageCode).IsRequired().HasMaxLength(10);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Description).HasColumnType("text");

        builder.HasIndex(x => new { x.ColorId, x.LanguageCode }).IsUnique();
    }
}
