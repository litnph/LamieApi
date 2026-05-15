using Lamie.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lamie.Infrastructure.Persistence.Configurations.MasterData;

public sealed class CollectionConfiguration : IEntityTypeConfiguration<Collection>
{
    public void Configure(EntityTypeBuilder<Collection> builder)
    {
        builder.ToTable("md_collections");
        builder.ConfigureEntityBase();

        builder.Property(x => x.IsActive).IsRequired();

        builder.Metadata.FindNavigation(nameof(Collection.Translations))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(c => c.Translations)
            .WithOne()
            .HasForeignKey(t => t.CollectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class CollectionTranslationConfiguration : IEntityTypeConfiguration<CollectionTranslation>
{
    public void Configure(EntityTypeBuilder<CollectionTranslation> builder)
    {
        builder.ToTable("md_collection_translations");
        builder.ConfigureEntityBase();
        builder.Property(x => x.CollectionId).IsRequired();
        builder.Property(x => x.LanguageCode).IsRequired().HasMaxLength(10);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Description).HasColumnType("text");

        builder.HasIndex(x => new { x.CollectionId, x.LanguageCode }).IsUnique();
    }
}
