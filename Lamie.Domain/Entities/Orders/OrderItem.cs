using Lamie.Domain.Exceptions;

namespace Lamie.Domain.Entities.Orders;

public class OrderItem : Entity
{
    public Guid OrderId { get; private set; }
    public Guid? ProductId { get; private set; }
    public string? ProductSku { get; private set; }
    public string ProductName { get; private set; } = default!;
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public decimal LineTotal { get; private set; }
    public string? Note { get; private set; }

    private OrderItem() { }

    internal OrderItem(
        Guid? productId,
        string? productSku,
        string productName,
        decimal unitPrice,
        int quantity,
        string? note)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new DomainException("ProductName is required");
        if (unitPrice < 0)
            throw new DomainException("Unit price cannot be negative");
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than 0");

        ProductId = productId;
        ProductSku = productSku;
        ProductName = productName.Trim();
        UnitPrice = unitPrice;
        Quantity = quantity;
        LineTotal = unitPrice * quantity;
        Note = note;
    }

    internal void Update(decimal unitPrice, int quantity, string? note)
    {
        if (unitPrice < 0)
            throw new DomainException("Unit price cannot be negative");
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than 0");

        UnitPrice = unitPrice;
        Quantity = quantity;
        LineTotal = unitPrice * quantity;
        Note = note;
    }
}
