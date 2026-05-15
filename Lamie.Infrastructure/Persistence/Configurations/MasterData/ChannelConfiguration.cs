using Lamie.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lamie.Infrastructure.Persistence.Configurations.MasterData;

public sealed class ChannelConfiguration : IEntityTypeConfiguration<Channel>
{
    public void Configure(EntityTypeBuilder<Channel> builder)
    {
        builder.ToTable("md_channels");
        builder.ConfigureEntityBase();

        builder.Property(x => x.Code).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.IconUrl).HasMaxLength(500);
        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.SortOrder).IsRequired();

        builder.HasIndex(x => x.Code).IsUnique();
    }
}
