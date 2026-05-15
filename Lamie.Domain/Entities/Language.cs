namespace Lamie.Domain.Entities;

/// <summary>
/// Lookup table for supported language codes (ISO 639-1 / BCP 47).
/// Uses Code as natural PK so we don't introduce a surrogate Guid.
/// Not soft-deletable; always small in size.
/// </summary>
public class Language
{
    public string Code { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public bool IsActive { get; private set; }

    private Language() { }

    public Language(string code, string name, bool isActive = true)
    {
        Code = code;
        Name = name;
        IsActive = isActive;
    }

    public void Rename(string name) => Name = name;
    public void SetActive(bool isActive) => IsActive = isActive;
}
