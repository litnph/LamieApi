namespace Lamie.Application.MasterData.Tags;

public sealed record TagTranslationInput(
    string LanguageCode,
    string Name,
    string? Description
);

