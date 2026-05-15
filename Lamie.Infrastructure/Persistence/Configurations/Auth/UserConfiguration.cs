using Lamie.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lamie.Infrastructure.Persistence.Configurations.Auth;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("auth_users");
        builder.ConfigureEntityBase();

        builder.Property(x => x.Email).IsRequired().HasMaxLength(255);
        builder.Property(x => x.UserName).IsRequired().HasMaxLength(64);
        builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(255);
        builder.Property(x => x.FullName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Phone).HasMaxLength(30);
        builder.Property(x => x.Role).IsRequired().HasConversion<int>();
        builder.Property(x => x.IsActive).IsRequired();

        builder.HasIndex(x => x.Email).IsUnique();
        builder.HasIndex(x => x.UserName).IsUnique();
    }
}

public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("auth_refresh_tokens");
        builder.ConfigureEntityBase();

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.TokenHash).IsRequired().HasMaxLength(128);
        builder.Property(x => x.ExpiresAt).IsRequired();
        builder.Property(x => x.CreatedByIp).HasMaxLength(64);
        builder.Property(x => x.RevokedByIp).HasMaxLength(64);

        builder.HasIndex(x => x.TokenHash).IsUnique();
        builder.HasIndex(x => x.UserId);
    }
}
