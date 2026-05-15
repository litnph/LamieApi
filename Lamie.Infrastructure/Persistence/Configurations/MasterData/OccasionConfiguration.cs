using Lamie.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lamie.Infrastructure.Persistence.Configurations.MasterData;

public sealed class OccasionConfiguration : IEntityTypeConfiguration<Occasion>
{
    public void Configure(EntityTypeBuilder<Occasion> builder)
    {
        builder.ToTable("md_occasions");
        builder.ConfigureEntityBase();

        builder.Property(x => x.IsActive).IsRequired();

        builder.Metadata.FindNavigation(nameof(Occasion.Translations))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(c => c.Translations)
            .WithOne()
            .HasForeignKey(t => t.OccasionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class OccasionTranslationConfiguration : IEntityTypeConfiguration<OccasionTranslation>
{
    public void Configure(EntityTypeBuilder<OccasionTranslation> builder)
    {
        builder.ToTable("md_occasion_translations");
        builder.ConfigureEntityBase();
        builder.Property(x => x.OccasionId).IsRequired();
        builder.Property(x => x.LanguageCode).IsRequired().HasMaxLength(10);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Description).HasColumnType("text");

        builder.HasIndex(x => new { x.OccasionId, x.LanguageCode }).IsUnique();
    }
}
