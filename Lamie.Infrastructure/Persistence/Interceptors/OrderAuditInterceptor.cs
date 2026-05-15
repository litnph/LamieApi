using Lamie.Domain.Entities.Orders;
using Lamie.Shared.Auth;
using Lamie.Shared.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Lamie.Infrastructure.Persistence.Interceptors;

/// <summary>
/// Records every change to <see cref="Order"/>, <see cref="OrderItem"/> and <see cref="OrderImage"/>
/// in <see cref="OrderChangeLog"/> for the order detail history view.
/// Hooked into the same SaveChanges so logs are written in the same transaction.
/// </summary>
public sealed class OrderAuditInterceptor : SaveChangesInterceptor
{
    private static readonly HashSet<string> SkipProperties = new(StringComparer.Ordinal)
    {
        "Id",
        "OrderId",
        "CreatedAt",
        "CreatedBy",
        "CreatedName",
        "UpdatedAt",
        "UpdatedBy",
        "UpdatedName",
        "IsDeleted",
        "DeletedAt",
        "DeletedBy",
        "DeletedName",
    };

    private readonly IUserContext _userContext;
    private readonly IClock _clock;

    public OrderAuditInterceptor(IUserContext userContext, IClock clock)
    {
        _userContext = userContext;
        _clock = clock;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        WriteLogs(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        WriteLogs(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void WriteLogs(DbContext? context)
    {
        if (context is null) return;

        var now = _clock.UtcNow;
        var userId = _userContext.UserId;
        var userName = _userContext.UserName ?? _userContext.Email ?? "system";

        var logs = new List<OrderChangeLog>();

        foreach (var entry in context.ChangeTracker.Entries().ToList())
        {
            var orderId = ResolveOrderId(entry);
            if (orderId is null) continue;

            var entityName = entry.Entity.GetType().Name;

            switch (entry.State)
            {
                case EntityState.Added:
                    logs.Add(new OrderChangeLog(
                        orderId.Value, entityName, "*", null, "created",
                        "Created", userId, userName, now));
                    break;

                case EntityState.Modified:
                    foreach (var prop in entry.Properties)
                    {
                        if (!prop.IsModified) continue;
                        var name = prop.Metadata.Name;
                        if (SkipProperties.Contains(name)) continue;

                        var oldVal = prop.OriginalValue?.ToString();
                        var newVal = prop.CurrentValue?.ToString();
                        if (oldVal == newVal) continue;

                        logs.Add(new OrderChangeLog(
                            orderId.Value, entityName, name, oldVal, newVal,
                            "Modified", userId, userName, now));
                    }
                    break;

                case EntityState.Deleted:
                    logs.Add(new OrderChangeLog(
                        orderId.Value, entityName, "*", "deleted", null,
                        "Deleted", userId, userName, now));
                    break;
            }
        }

        if (logs.Count > 0)
        {
            context.Set<OrderChangeLog>().AddRange(logs);
        }
    }

    private static Guid? ResolveOrderId(EntityEntry entry)
    {
        return entry.Entity switch
        {
            Order o => o.Id,
            OrderItem i => i.OrderId,
            OrderImage img => img.OrderId,
            _ => null,
        };
    }
}
