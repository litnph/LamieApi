using Lamie.Domain.Entities.Orders;

namespace Lamie.Application.Orders.Dtos;

public sealed record OrderItemDto
{
    public Guid Id { get; init; }
    public Guid? ProductId { get; init; }
    public string? ProductSku { get; init; }
    public string ProductName { get; init; } = default!;
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; }
    public decimal LineTotal { get; init; }
    public string? Note { get; init; }
}

public sealed record OrderImageDto
{
    public Guid Id { get; init; }
    public string ImageUrl { get; init; } = default!;
    public int SortOrder { get; init; }
    public string? Description { get; init; }
}

public sealed record OrderChangeLogDto
{
    public Guid Id { get; init; }
    public string EntityName { get; init; } = default!;
    public string FieldName { get; init; } = default!;
    public string? OldValue { get; init; }
    public string? NewValue { get; init; }
    public string ChangeType { get; init; } = default!;
    public Guid? ChangedById { get; init; }
    public string? ChangedByName { get; init; }
    public DateTime ChangedAt { get; init; }
    public string? Note { get; init; }
}

public sealed record OrderListItemDto
{
    public Guid Id { get; init; }
    public string OrderCode { get; init; } = default!;
    public string OrdererName { get; init; } = default!;
    public string OrdererPhone { get; init; } = default!;
    public Guid ChannelId { get; init; }
    public string RecipientName { get; init; } = default!;
    public string RecipientPhone { get; init; } = default!;
    public bool PickupAtShop { get; init; }
    public string? DeliveryAddress { get; init; }
    public DateTime DeliveryAt { get; init; }
    public decimal DepositAmount { get; init; }
    public decimal ShippingFee { get; init; }
    public decimal? ShippingFeeActual { get; init; }
    public decimal SubTotal { get; init; }
    public decimal TotalAmount { get; init; }
    public PaymentStatus PaymentStatus { get; init; }
    public OrderStatus OrderStatus { get; init; }
    public DateTime CreatedAt { get; init; }
}

public sealed record OrderDetailDto
{
    public Guid Id { get; init; }
    public string OrderCode { get; init; } = default!;
    public string OrdererName { get; init; } = default!;
    public string OrdererPhone { get; init; } = default!;
    public Guid ChannelId { get; init; }
    public string RecipientName { get; init; } = default!;
    public string RecipientPhone { get; init; } = default!;
    public bool PickupAtShop { get; init; }
    public string? DeliveryAddress { get; init; }
    public double? DeliveryLatitude { get; init; }
    public double? DeliveryLongitude { get; init; }
    public DateTime DeliveryAt { get; init; }
    public decimal DepositAmount { get; init; }
    public string? Description { get; init; }
    public string? ContentNote { get; init; }
    public decimal ShippingFee { get; init; }
    public decimal? ShippingFeeActual { get; init; }
    public decimal SubTotal { get; init; }
    public decimal TotalAmount { get; init; }
    public PaymentStatus PaymentStatus { get; init; }
    public OrderStatus OrderStatus { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public IReadOnlyList<OrderItemDto> Items { get; init; } = [];
    public IReadOnlyList<OrderImageDto> Images { get; init; } = [];
    public IReadOnlyList<OrderChangeLogDto> ChangeLogs { get; init; } = [];
}

public sealed record OrderCalendarItemDto
{
    public Guid Id { get; init; }
    public string OrderCode { get; init; } = default!;
    public string RecipientName { get; init; } = default!;
    public string RecipientPhone { get; init; } = default!;
    public DateTime DeliveryAt { get; init; }
    public bool PickupAtShop { get; init; }
    public string? DeliveryAddress { get; init; }
    public OrderStatus OrderStatus { get; init; }
    public PaymentStatus PaymentStatus { get; init; }
    public decimal TotalAmount { get; init; }
}

public sealed record OrderDeliveryLocationDto
{
    public Guid Id { get; init; }
    public string OrderCode { get; init; } = default!;
    public string RecipientName { get; init; } = default!;
    public string? DeliveryAddress { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public DateTime DeliveryAt { get; init; }
    public OrderStatus OrderStatus { get; init; }
}

internal static class OrderMapper
{
    public static OrderDetailDto ToDetail(Domain.Entities.Orders.Order order, IReadOnlyList<Domain.Entities.Orders.OrderChangeLog> logs) => new()
    {
        Id = order.Id,
        OrderCode = order.OrderCode,
        OrdererName = order.OrdererName,
        OrdererPhone = order.OrdererPhone,
        ChannelId = order.ChannelId,
        RecipientName = order.RecipientName,
        RecipientPhone = order.RecipientPhone,
        PickupAtShop = order.PickupAtShop,
        DeliveryAddress = order.DeliveryAddress,
        DeliveryLatitude = order.DeliveryLatitude,
        DeliveryLongitude = order.DeliveryLongitude,
        DeliveryAt = order.DeliveryAt,
        DepositAmount = order.DepositAmount,
        Description = order.Description,
        ContentNote = order.ContentNote,
        ShippingFee = order.ShippingFee,
        ShippingFeeActual = order.ShippingFeeActual,
        SubTotal = order.SubTotal,
        TotalAmount = order.TotalAmount,
        PaymentStatus = order.PaymentStatus,
        OrderStatus = order.OrderStatus,
        CreatedAt = order.CreatedAt,
        UpdatedAt = order.UpdatedAt,
        Items = order.Items.Select(i => new OrderItemDto
        {
            Id = i.Id,
            ProductId = i.ProductId,
            ProductSku = i.ProductSku,
            ProductName = i.ProductName,
            UnitPrice = i.UnitPrice,
            Quantity = i.Quantity,
            LineTotal = i.LineTotal,
            Note = i.Note,
        }).ToList(),
        Images = order.Images.OrderBy(i => i.SortOrder).Select(i => new OrderImageDto
        {
            Id = i.Id,
            ImageUrl = i.ImageUrl,
            SortOrder = i.SortOrder,
            Description = i.Description,
        }).ToList(),
        ChangeLogs = logs.OrderByDescending(l => l.ChangedAt).Select(l => new OrderChangeLogDto
        {
            Id = l.Id,
            EntityName = l.EntityName,
            FieldName = l.FieldName,
            OldValue = l.OldValue,
            NewValue = l.NewValue,
            ChangeType = l.ChangeType,
            ChangedById = l.ChangedById,
            ChangedByName = l.ChangedByName,
            ChangedAt = l.ChangedAt,
            Note = l.Note,
        }).ToList(),
    };

    public static OrderListItemDto ToListItem(Domain.Entities.Orders.Order order) => new()
    {
        Id = order.Id,
        OrderCode = order.OrderCode,
        OrdererName = order.OrdererName,
        OrdererPhone = order.OrdererPhone,
        ChannelId = order.ChannelId,
        RecipientName = order.RecipientName,
        RecipientPhone = order.RecipientPhone,
        PickupAtShop = order.PickupAtShop,
        DeliveryAddress = order.DeliveryAddress,
        DeliveryAt = order.DeliveryAt,
        DepositAmount = order.DepositAmount,
        ShippingFee = order.ShippingFee,
        ShippingFeeActual = order.ShippingFeeActual,
        SubTotal = order.SubTotal,
        TotalAmount = order.TotalAmount,
        PaymentStatus = order.PaymentStatus,
        OrderStatus = order.OrderStatus,
        CreatedAt = order.CreatedAt,
    };

    public static OrderCalendarItemDto ToCalendarItem(Domain.Entities.Orders.Order order) => new()
    {
        Id = order.Id,
        OrderCode = order.OrderCode,
        RecipientName = order.RecipientName,
        RecipientPhone = order.RecipientPhone,
        DeliveryAt = order.DeliveryAt,
        PickupAtShop = order.PickupAtShop,
        DeliveryAddress = order.DeliveryAddress,
        OrderStatus = order.OrderStatus,
        PaymentStatus = order.PaymentStatus,
        TotalAmount = order.TotalAmount,
    };
}
