using Lamie.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lamie.Infrastructure.Persistence.Configurations.Orders;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("ord_orders");
        builder.ConfigureEntityBase();

        builder.Property(x => x.OrderCode).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.OrderCode).IsUnique();

        builder.Property(x => x.OrdererName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.OrdererPhone).IsRequired().HasMaxLength(30);
        builder.Property(x => x.ChannelId).IsRequired();
        builder.Property(x => x.RecipientName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.RecipientPhone).IsRequired().HasMaxLength(30);

        builder.Property(x => x.PickupAtShop).IsRequired();
        builder.Property(x => x.DeliveryAddress).HasMaxLength(500);
        builder.Property(x => x.DeliveryLatitude);
        builder.Property(x => x.DeliveryLongitude);
        builder.Property(x => x.DeliveryAt).IsRequired();

        builder.Property(x => x.DepositAmount).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(x => x.Description).HasColumnType("text");
        builder.Property(x => x.ContentNote).HasColumnType("text");
        builder.Property(x => x.ShippingFee).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(x => x.ShippingFeeActual).HasColumnType("numeric(18,2)");
        builder.Property(x => x.SubTotal).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(x => x.TotalAmount).HasColumnType("numeric(18,2)").IsRequired();

        builder.Property(x => x.PaymentStatus).HasConversion<int>().IsRequired();
        builder.Property(x => x.OrderStatus).HasConversion<int>().IsRequired();

        builder.HasIndex(x => x.OrderStatus);
        builder.HasIndex(x => x.PaymentStatus);
        builder.HasIndex(x => x.ChannelId);
        builder.HasIndex(x => x.DeliveryAt);
        builder.HasIndex(x => x.OrdererPhone);
        builder.HasIndex(x => x.RecipientPhone);

        builder.Metadata.FindNavigation(nameof(Order.Items))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.Metadata.FindNavigation(nameof(Order.Images))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(o => o.Images)
            .WithOne()
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("ord_order_items");
        builder.ConfigureEntityBase();

        builder.Property(x => x.OrderId).IsRequired();
        builder.Property(x => x.ProductId);
        builder.Property(x => x.ProductSku).HasMaxLength(100);
        builder.Property(x => x.ProductName).IsRequired().HasMaxLength(255);
        builder.Property(x => x.UnitPrice).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(x => x.Quantity).IsRequired();
        builder.Property(x => x.LineTotal).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(x => x.Note).HasColumnType("text");

        builder.HasIndex(x => x.OrderId);
    }
}

public sealed class OrderImageConfiguration : IEntityTypeConfiguration<OrderImage>
{
    public void Configure(EntityTypeBuilder<OrderImage> builder)
    {
        builder.ToTable("ord_order_images");
        builder.ConfigureEntityBase();

        builder.Property(x => x.OrderId).IsRequired();
        builder.Property(x => x.ImageUrl).IsRequired().HasMaxLength(1000);
        builder.Property(x => x.SortOrder).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);

        builder.HasIndex(x => x.OrderId);
    }
}

public sealed class OrderChangeLogConfiguration : IEntityTypeConfiguration<OrderChangeLog>
{
    public void Configure(EntityTypeBuilder<OrderChangeLog> builder)
    {
        builder.ToTable("ord_order_change_logs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.OrderId).IsRequired();
        builder.Property(x => x.EntityName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.FieldName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.OldValue).HasColumnType("text");
        builder.Property(x => x.NewValue).HasColumnType("text");
        builder.Property(x => x.ChangeType).IsRequired().HasMaxLength(20);
        builder.Property(x => x.ChangedById);
        builder.Property(x => x.ChangedByName).HasMaxLength(200);
        builder.Property(x => x.ChangedAt).IsRequired();
        builder.Property(x => x.Note).HasMaxLength(500);

        builder.HasIndex(x => x.OrderId);
        builder.HasIndex(x => new { x.OrderId, x.ChangedAt });
    }
}
