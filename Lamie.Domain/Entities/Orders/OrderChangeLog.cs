using Lamie.Shared.Guids;

namespace Lamie.Domain.Entities.Orders;

/// <summary>
/// Generic audit log for any change applied to an order or its child collections.
/// Persisted by the OrderAuditInterceptor on every SaveChanges.
/// Not an Entity - we never update or soft-delete a log row.
/// </summary>
public class OrderChangeLog
{
    public Guid Id { get; private set; } = GuidV7.NewGuid();
    public Guid OrderId { get; private set; }
    public string EntityName { get; private set; } = default!;
    public string FieldName { get; private set; } = default!;
    public string? OldValue { get; private set; }
    public string? NewValue { get; private set; }
    public string ChangeType { get; private set; } = default!;
    public Guid? ChangedById { get; private set; }
    public string? ChangedByName { get; private set; }
    public DateTime ChangedAt { get; private set; }
    public string? Note { get; private set; }

    private OrderChangeLog() { }

    public OrderChangeLog(
        Guid orderId,
        string entityName,
        string fieldName,
        string? oldValue,
        string? newValue,
        string changeType,
        Guid? changedById,
        string? changedByName,
        DateTime changedAt,
        string? note = null)
    {
        OrderId = orderId;
        EntityName = entityName;
        FieldName = fieldName;
        OldValue = oldValue;
        NewValue = newValue;
        ChangeType = changeType;
        ChangedById = changedById;
        ChangedByName = changedByName;
        ChangedAt = changedAt;
        Note = note;
    }
}
