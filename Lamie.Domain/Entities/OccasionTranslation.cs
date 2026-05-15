namespace Lamie.Domain.Entities;

public class OccasionTranslation : Entity
{
    public Guid OccasionId { get; private set; }
    public string LanguageCode { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }

    private OccasionTranslation() { }

    internal OccasionTranslation(string languageCode, string name, string? description)
    {
        LanguageCode = languageCode;
        Name = name;
        Description = description;
    }

    internal void Update(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}
