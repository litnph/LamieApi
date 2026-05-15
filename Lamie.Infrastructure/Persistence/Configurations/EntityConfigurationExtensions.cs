using Lamie.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lamie.Infrastructure.Persistence.Configurations;

internal static class EntityConfigurationExtensions
{
    /// <summary>
    /// Applies common conventions for entities deriving from <see cref="Entity"/>:
    /// Guid primary key, audit columns, soft-delete column, and a global query filter
    /// hiding soft-deleted rows by default.
    /// </summary>
    public static EntityTypeBuilder<T> ConfigureEntityBase<T>(this EntityTypeBuilder<T> builder) where T : Entity
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.CreatedBy);
        builder.Property(e => e.CreatedName).HasMaxLength(200);

        builder.Property(e => e.UpdatedAt);
        builder.Property(e => e.UpdatedBy);
        builder.Property(e => e.UpdatedName).HasMaxLength(200);

        builder.Property(e => e.IsDeleted).IsRequired().HasDefaultValue(false);
        builder.Property(e => e.DeletedAt);
        builder.Property(e => e.DeletedBy);
        builder.Property(e => e.DeletedName).HasMaxLength(200);

        builder.HasQueryFilter(e => !e.IsDeleted);

        return builder;
    }
}
