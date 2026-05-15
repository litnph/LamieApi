using Lamie.Domain.Exceptions;

namespace Lamie.Domain.Entities;

/// <summary>
/// Sales/communication channel master data (Meta Business, Tiktok, Zalo, ...).
/// Referenced by Order to record where the order came from.
/// </summary>
public class Channel : Entity
{
    public string Code { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string? IconUrl { get; private set; }
    public bool IsActive { get; private set; }
    public int SortOrder { get; private set; }

    private Channel() { }

    public Channel(string code, string name, string? iconUrl, int sortOrder, bool isActive = true)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new DomainException("Channel code is required");
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Channel name is required");

        Code = code.Trim().ToUpperInvariant();
        Name = name.Trim();
        IconUrl = iconUrl;
        SortOrder = sortOrder;
        IsActive = isActive;
    }

    public void Update(string name, string? iconUrl, int sortOrder, bool isActive)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Channel name is required");

        Name = name.Trim();
        IconUrl = iconUrl;
        SortOrder = sortOrder;
        IsActive = isActive;
    }
}
