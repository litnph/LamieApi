namespace Lamie.Application.Settings.Attributes.Tags;

public sealed record TagTranslationInput
{
    public string LanguageCode { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
}

public sealed record TagDto
{
    public Guid Id { get; init; }
    public bool IsActive { get; init; }
    public IReadOnlyList<TagTranslationDto> Translations { get; init; } = [];
}

public sealed record TagTranslationDto
{
    public string LanguageCode { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
}
