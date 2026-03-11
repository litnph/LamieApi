using MediatR;

namespace Lamie.Application.MasterData.Tags;

public sealed record CreateTagCommand(
    bool IsActive,
    List<TagTranslationInput> Translations
) : IRequest<int>;

public sealed record UpdateTagCommand(
    int Id,
    bool IsActive,
    List<TagTranslationInput> Translations
) : IRequest;

public sealed record DeleteTagCommand(int Id) : IRequest;

