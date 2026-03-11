namespace Lamie.Domain.Entities;

public abstract class AuditableEntity
{
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public int CreatedBy { get; private set; } = -99;
    public string CreatedName { get; private set; } = "dev";

    public DateTime? UpdatedAt { get; private set; }
    public int UpdatedBy { get; private set; } = -99;
    public string UpdatedName { get; private set; } = "dev";

    public void SetCreated(int createdBy, string createdName, DateTime? createdAtUtc = null)
    {
        CreatedBy = createdBy;
        CreatedName = createdName;
        CreatedAt = createdAtUtc ?? DateTime.UtcNow;
    }

    public void SetUpdated(int updatedBy, string updatedName, DateTime? updatedAtUtc = null)
    {
        UpdatedBy = updatedBy;
        UpdatedName = updatedName;
        UpdatedAt = updatedAtUtc ?? DateTime.UtcNow;
    }
}

