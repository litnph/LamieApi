using Lamie.Domain.Entities.Orders;

namespace Lamie.Domain.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Order?> GetByCodeAsync(string orderCode, CancellationToken cancellationToken = default);
    Task<int> CountByDatePrefixAsync(string codePrefix, CancellationToken cancellationToken = default);
    Task AddAsync(Order order, CancellationToken cancellationToken = default);
    Task UpdateAsync(Order order, CancellationToken cancellationToken = default);
    Task DeleteAsync(Order order, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Order> Items, int TotalCount)> ListAsync(OrderListFilter filter, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Order>> GetByDeliveryDateAsync(DateOnly date, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrderChangeLog>> GetChangeLogsAsync(Guid orderId, CancellationToken cancellationToken = default);
}

public sealed record OrderListFilter
{
    public OrderStatus? OrderStatus { get; init; }
    public PaymentStatus? PaymentStatus { get; init; }
    public Guid? ChannelId { get; init; }
    public DateTime? DeliveryFrom { get; init; }
    public DateTime? DeliveryTo { get; init; }
    public DateTime? CreatedFrom { get; init; }
    public DateTime? CreatedTo { get; init; }
    public string? Phone { get; init; }
    public string? Search { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
