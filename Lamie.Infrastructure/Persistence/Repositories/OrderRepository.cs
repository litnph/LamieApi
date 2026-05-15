using Lamie.Domain.Entities.Orders;
using Lamie.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lamie.Infrastructure.Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Orders
            .Include(o => o.Items)
            .Include(o => o.Images)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public Task<Order?> GetByCodeAsync(string orderCode, CancellationToken cancellationToken = default)
    {
        return _context.Orders
            .Include(o => o.Items)
            .Include(o => o.Images)
            .FirstOrDefaultAsync(o => o.OrderCode == orderCode, cancellationToken);
    }

    public Task<int> CountByDatePrefixAsync(string codePrefix, CancellationToken cancellationToken = default)
    {
        return _context.Orders
            .IgnoreQueryFilters()
            .CountAsync(o => o.OrderCode.StartsWith(codePrefix), cancellationToken);
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Order order, CancellationToken cancellationToken = default)
    {
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<Order> Items, int TotalCount)> ListAsync(OrderListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Orders.AsQueryable();

        if (filter.OrderStatus.HasValue) query = query.Where(o => o.OrderStatus == filter.OrderStatus.Value);
        if (filter.PaymentStatus.HasValue) query = query.Where(o => o.PaymentStatus == filter.PaymentStatus.Value);
        if (filter.ChannelId.HasValue) query = query.Where(o => o.ChannelId == filter.ChannelId.Value);
        if (filter.DeliveryFrom.HasValue) query = query.Where(o => o.DeliveryAt >= filter.DeliveryFrom.Value);
        if (filter.DeliveryTo.HasValue) query = query.Where(o => o.DeliveryAt <= filter.DeliveryTo.Value);
        if (filter.CreatedFrom.HasValue) query = query.Where(o => o.CreatedAt >= filter.CreatedFrom.Value);
        if (filter.CreatedTo.HasValue) query = query.Where(o => o.CreatedAt <= filter.CreatedTo.Value);

        if (!string.IsNullOrWhiteSpace(filter.Phone))
        {
            var phone = filter.Phone.Trim();
            query = query.Where(o => o.OrdererPhone.Contains(phone) || o.RecipientPhone.Contains(phone));
        }

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(o =>
                o.OrderCode.Contains(search) ||
                o.OrdererName.Contains(search) ||
                o.RecipientName.Contains(search));
        }

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Include(o => o.Items)
            .Include(o => o.Images)
            .ToListAsync(cancellationToken);

        return (items, total);
    }

    public async Task<IReadOnlyList<Order>> GetByDeliveryDateAsync(DateOnly date, CancellationToken cancellationToken = default)
    {
        var startUtc = date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var endUtc = startUtc.AddDays(1);

        return await _context.Orders
            .Where(o => o.DeliveryAt >= startUtc && o.DeliveryAt < endUtc)
            .OrderBy(o => o.DeliveryAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<OrderChangeLog>> GetChangeLogsAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await _context.OrderChangeLogs
            .AsNoTracking()
            .Where(l => l.OrderId == orderId)
            .OrderByDescending(l => l.ChangedAt)
            .ToListAsync(cancellationToken);
    }
}
