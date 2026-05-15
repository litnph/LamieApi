using Lamie.Domain.Entities;
using Lamie.Shared.Auth;
using Lamie.Shared.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Lamie.Infrastructure.Persistence.Interceptors;

/// <summary>
/// Sets CreatedAt/UpdatedAt and the corresponding actor columns for all <see cref="Entity"/> changes,
/// and converts hard deletes into soft deletes (flips IsDeleted instead of removing the row).
/// </summary>
public sealed class AuditInterceptor : SaveChangesInterceptor
{
    private readonly IUserContext _userContext;
    private readonly IClock _clock;

    public AuditInterceptor(IUserContext userContext, IClock clock)
    {
        _userContext = userContext;
        _clock = clock;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        Apply(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        Apply(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void Apply(DbContext? context)
    {
        if (context is null) return;

        var now = _clock.UtcNow;
        var userId = _userContext.UserId;
        var userName = _userContext.UserName ?? _userContext.Email ?? "system";

        foreach (var entry in context.ChangeTracker.Entries<Entity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    SetCreated(entry, now, userId, userName);
                    SetUpdated(entry, now, userId, userName);
                    break;

                case EntityState.Modified:
                    PreserveCreated(entry);
                    SetUpdated(entry, now, userId, userName);
                    break;

                case EntityState.Deleted:
                    PreserveCreated(entry);
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = now;
                    entry.Entity.DeletedBy = userId;
                    entry.Entity.DeletedName = userName;
                    SetUpdated(entry, now, userId, userName);
                    break;
            }
        }
    }

    private static void SetCreated(EntityEntry<Entity> entry, DateTime now, Guid? userId, string userName)
    {
        entry.Entity.CreatedAt = now;
        entry.Entity.CreatedBy = userId;
        entry.Entity.CreatedName = userName;
    }

    private static void SetUpdated(EntityEntry<Entity> entry, DateTime now, Guid? userId, string userName)
    {
        entry.Entity.UpdatedAt = now;
        entry.Entity.UpdatedBy = userId;
        entry.Entity.UpdatedName = userName;
    }

    private static void PreserveCreated(EntityEntry<Entity> entry)
    {
        entry.Property(nameof(Entity.CreatedAt)).IsModified = false;
        entry.Property(nameof(Entity.CreatedBy)).IsModified = false;
        entry.Property(nameof(Entity.CreatedName)).IsModified = false;
    }
}
