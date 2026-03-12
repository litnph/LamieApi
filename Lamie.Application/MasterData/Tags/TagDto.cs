namespace Lamie.Application.MasterData.Tags;

public sealed record TagTranslationInput(string LanguageCode, string Name, string? Description);

public sealed record TagDto(int Id, bool IsActive, IReadOnlyList<TagTranslationDto> Translations);

public sealed record TagTranslationDto(string LanguageCode, string Name, string? Description);
