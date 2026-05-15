using Lamie.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lamie.Infrastructure.Persistence.Configurations.System;

public sealed class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.ToTable("sys_languages");
        builder.HasKey(x => x.Code);

        builder.Property(x => x.Code).IsRequired().HasMaxLength(10);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.IsActive).IsRequired();
    }
}
