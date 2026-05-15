using Lamie.Shared.Guids;

namespace Lamie.Domain.Entities;

/// <summary>
/// Base class for every persisted entity in the domain.
/// Owns the surrogate Id (Guid v7), the audit columns and the soft-delete flag.
/// All audit/soft-delete fields are written by EF interceptors in Infrastructure.
/// </summary>
public abstract class Entity : ISoftDelete, IAuditable
{
    public Guid Id { get; protected set; } = GuidV7.NewGuid();

    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public string? CreatedName { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public string? UpdatedName { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
    public string? DeletedName { get; set; }
}

public interface ISoftDelete
{
    bool IsDeleted { get; set; }
    DateTime? DeletedAt { get; set; }
    Guid? DeletedBy { get; set; }
    string? DeletedName { get; set; }
}

public interface IAuditable
{
    DateTime CreatedAt { get; set; }
    Guid? CreatedBy { get; set; }
    string? CreatedName { get; set; }
    DateTime? UpdatedAt { get; set; }
    Guid? UpdatedBy { get; set; }
    string? UpdatedName { get; set; }
}
