using Lamie.Domain.Exceptions;

namespace Lamie.Domain.Entities.Orders;

public class Order : Entity
{
    private readonly List<OrderItem> _items = new();
    private readonly List<OrderImage> _images = new();

    public string OrderCode { get; private set; } = default!;

    public string OrdererName { get; private set; } = default!;
    public string OrdererPhone { get; private set; } = default!;
    public Guid ChannelId { get; private set; }

    public string RecipientName { get; private set; } = default!;
    public string RecipientPhone { get; private set; } = default!;

    public bool PickupAtShop { get; private set; }
    public string? DeliveryAddress { get; private set; }
    public double? DeliveryLatitude { get; private set; }
    public double? DeliveryLongitude { get; private set; }
    public DateTime DeliveryAt { get; private set; }

    public decimal DepositAmount { get; private set; }
    public string? Description { get; private set; }
    public string? ContentNote { get; private set; }

    public decimal ShippingFee { get; private set; }
    public decimal? ShippingFeeActual { get; private set; }

    public decimal SubTotal { get; private set; }
    public decimal TotalAmount { get; private set; }

    public PaymentStatus PaymentStatus { get; private set; }
    public OrderStatus OrderStatus { get; private set; }

    public IReadOnlyCollection<OrderItem> Items => _items;
    public IReadOnlyCollection<OrderImage> Images => _images;

    private Order() { }

    public Order(
        string orderCode,
        string ordererName,
        string ordererPhone,
        Guid channelId,
        string recipientName,
        string recipientPhone,
        bool pickupAtShop,
        string? deliveryAddress,
        double? deliveryLatitude,
        double? deliveryLongitude,
        DateTime deliveryAt,
        decimal depositAmount,
        decimal shippingFee,
        string? description,
        string? contentNote)
    {
        if (string.IsNullOrWhiteSpace(orderCode))
            throw new DomainException("OrderCode is required");
        if (string.IsNullOrWhiteSpace(ordererName))
            throw new DomainException("OrdererName is required");
        if (string.IsNullOrWhiteSpace(ordererPhone))
            throw new DomainException("OrdererPhone is required");
        if (channelId == Guid.Empty)
            throw new DomainException("ChannelId is required");
        if (string.IsNullOrWhiteSpace(recipientName))
            throw new DomainException("RecipientName is required");
        if (string.IsNullOrWhiteSpace(recipientPhone))
            throw new DomainException("RecipientPhone is required");
        if (!pickupAtShop && string.IsNullOrWhiteSpace(deliveryAddress))
            throw new DomainException("Delivery address is required when not picking up at shop");
        if (depositAmount < 0)
            throw new DomainException("Deposit amount cannot be negative");
        if (shippingFee < 0)
            throw new DomainException("Shipping fee cannot be negative");

        OrderCode = orderCode.Trim();
        OrdererName = ordererName.Trim();
        OrdererPhone = ordererPhone.Trim();
        ChannelId = channelId;
        RecipientName = recipientName.Trim();
        RecipientPhone = recipientPhone.Trim();
        PickupAtShop = pickupAtShop;
        DeliveryAddress = pickupAtShop ? deliveryAddress : deliveryAddress?.Trim();
        DeliveryLatitude = pickupAtShop ? null : deliveryLatitude;
        DeliveryLongitude = pickupAtShop ? null : deliveryLongitude;
        DeliveryAt = deliveryAt;
        DepositAmount = depositAmount;
        ShippingFee = pickupAtShop ? 0m : shippingFee;
        Description = description;
        ContentNote = contentNote;

        OrderStatus = OrderStatus.Created;
        PaymentStatus = depositAmount > 0 ? PaymentStatus.Deposited : PaymentStatus.Unpaid;

        Recalculate();
    }

    public void UpdateBasicInfo(
        string ordererName,
        string ordererPhone,
        Guid channelId,
        string recipientName,
        string recipientPhone,
        DateTime deliveryAt,
        decimal depositAmount,
        string? description,
        string? contentNote)
    {
        if (string.IsNullOrWhiteSpace(ordererName))
            throw new DomainException("OrdererName is required");
        if (string.IsNullOrWhiteSpace(ordererPhone))
            throw new DomainException("OrdererPhone is required");
        if (channelId == Guid.Empty)
            throw new DomainException("ChannelId is required");
        if (string.IsNullOrWhiteSpace(recipientName))
            throw new DomainException("RecipientName is required");
        if (string.IsNullOrWhiteSpace(recipientPhone))
            throw new DomainException("RecipientPhone is required");
        if (depositAmount < 0)
            throw new DomainException("Deposit amount cannot be negative");

        OrdererName = ordererName.Trim();
        OrdererPhone = ordererPhone.Trim();
        ChannelId = channelId;
        RecipientName = recipientName.Trim();
        RecipientPhone = recipientPhone.Trim();
        DeliveryAt = deliveryAt;
        DepositAmount = depositAmount;
        Description = description;
        ContentNote = contentNote;
    }

    public void SetDeliveryLocation(string? address, double? latitude, double? longitude)
    {
        if (PickupAtShop)
        {
            DeliveryAddress = null;
            DeliveryLatitude = null;
            DeliveryLongitude = null;
            return;
        }

        if (string.IsNullOrWhiteSpace(address))
            throw new DomainException("Delivery address is required when not picking up at shop");

        DeliveryAddress = address.Trim();
        DeliveryLatitude = latitude;
        DeliveryLongitude = longitude;
    }

    public void MarkPickupAtShop(bool pickupAtShop)
    {
        PickupAtShop = pickupAtShop;
        if (pickupAtShop)
        {
            DeliveryAddress = null;
            DeliveryLatitude = null;
            DeliveryLongitude = null;
            ShippingFee = 0m;
            ShippingFeeActual = null;
            Recalculate();
        }
    }

    public void ChangeShippingFee(decimal estimated, decimal? actual)
    {
        if (PickupAtShop)
        {
            ShippingFee = 0m;
            ShippingFeeActual = null;
            Recalculate();
            return;
        }

        if (estimated < 0) throw new DomainException("Shipping fee cannot be negative");
        if (actual.HasValue && actual.Value < 0) throw new DomainException("Actual shipping fee cannot be negative");

        ShippingFee = estimated;
        ShippingFeeActual = actual;
        Recalculate();
    }

    public OrderItem AddItem(Guid? productId, string? productSku, string productName, decimal unitPrice, int quantity, string? note)
    {
        var item = new OrderItem(productId, productSku, productName, unitPrice, quantity, note);
        _items.Add(item);
        Recalculate();
        return item;
    }

    public void RemoveItem(Guid itemId)
    {
        var item = _items.FirstOrDefault(x => x.Id == itemId);
        if (item is null) return;
        _items.Remove(item);
        Recalculate();
    }

    public void UpdateItem(Guid itemId, decimal unitPrice, int quantity, string? note)
    {
        var item = _items.FirstOrDefault(x => x.Id == itemId)
            ?? throw new DomainException($"Order item {itemId} not found");

        item.Update(unitPrice, quantity, note);
        Recalculate();
    }

    public void ClearItems()
    {
        _items.Clear();
        Recalculate();
    }

    public OrderImage AddImage(string imageUrl, int sortOrder, string? description)
    {
        var image = new OrderImage(imageUrl, sortOrder, description);
        _images.Add(image);
        return image;
    }

    public void RemoveImage(Guid imageId)
    {
        var image = _images.FirstOrDefault(x => x.Id == imageId);
        if (image is null) return;
        _images.Remove(image);
    }

    public void ChangeStatus(OrderStatus next)
    {
        if (next == OrderStatus) return;

        if (!IsValidStatusTransition(OrderStatus, next))
            throw new DomainException($"Cannot transition order from {OrderStatus} to {next}");

        OrderStatus = next;
    }

    public void ChangePaymentStatus(PaymentStatus next)
    {
        if (next == PaymentStatus) return;

        if (!IsValidPaymentTransition(PaymentStatus, next))
            throw new DomainException($"Cannot transition payment from {PaymentStatus} to {next}");

        PaymentStatus = next;
    }

    private static bool IsValidStatusTransition(OrderStatus from, OrderStatus to)
    {
        if (from == OrderStatus.Cancelled || from == OrderStatus.Completed)
            return false;

        return (from, to) switch
        {
            (OrderStatus.Created, OrderStatus.Producing) => true,
            (OrderStatus.Created, OrderStatus.Cancelled) => true,
            (OrderStatus.Producing, OrderStatus.Shipping) => true,
            (OrderStatus.Producing, OrderStatus.Cancelled) => true,
            (OrderStatus.Shipping, OrderStatus.Completed) => true,
            (OrderStatus.Shipping, OrderStatus.Cancelled) => true,
            _ => false,
        };
    }

    private static bool IsValidPaymentTransition(PaymentStatus from, PaymentStatus to)
    {
        return (from, to) switch
        {
            (PaymentStatus.Unpaid, PaymentStatus.Deposited) => true,
            (PaymentStatus.Unpaid, PaymentStatus.Paid) => true,
            (PaymentStatus.Deposited, PaymentStatus.Paid) => true,
            (PaymentStatus.Paid, PaymentStatus.Deposited) => false,
            (PaymentStatus.Deposited, PaymentStatus.Unpaid) => false,
            _ => false,
        };
    }

    private void Recalculate()
    {
        SubTotal = _items.Sum(i => i.LineTotal);
        var ship = ShippingFeeActual ?? ShippingFee;
        TotalAmount = SubTotal + ship;
    }
}
