using Lamie.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lamie.Infrastructure.Persistence.Configurations.MasterData;

public sealed class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("md_tags");
        builder.ConfigureEntityBase();
        builder.Property(x => x.IsActive).IsRequired();

        builder.Metadata.FindNavigation(nameof(Tag.Translations))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(t => t.Translations)
            .WithOne()
            .HasForeignKey(t => t.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class TagTranslationConfiguration : IEntityTypeConfiguration<TagTranslation>
{
    public void Configure(EntityTypeBuilder<TagTranslation> builder)
    {
        builder.ToTable("md_tag_translations");
        builder.ConfigureEntityBase();
        builder.Property(x => x.TagId).IsRequired();
        builder.Property(x => x.LanguageCode).IsRequired().HasMaxLength(10);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Description).HasColumnType("text");

        builder.HasIndex(x => new { x.TagId, x.LanguageCode }).IsUnique();
    }
}
