using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Entities;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.Settings.Attributes.Colors;

public sealed record CreateColorCommand(string HexCode, string RgbCode, bool IsActive, List<ColorTranslationInput> Translations) : IRequest<Guid>;

public sealed record UpdateColorCommand(Guid Id, string HexCode, string RgbCode, bool IsActive, List<ColorTranslationInput> Translations) : IRequest;

public sealed record DeleteColorCommand(Guid Id) : IRequest;

public sealed class CreateColorHandler : IRequestHandler<CreateColorCommand, Guid>
{
    private readonly IColorRepository _repository;
    private readonly ILanguageRepository _languageRepository;

    public CreateColorHandler(IColorRepository repository, ILanguageRepository languageRepository)
    {
        _repository = repository;
        _languageRepository = languageRepository;
    }

    public async Task<Guid> Handle(CreateColorCommand request, CancellationToken cancellationToken)
    {
        ValidateColorCodes(request.HexCode, request.RgbCode);
        await TranslationValidation.EnsureValidAsync(
            request.Translations, t => t.LanguageCode, t => t.Name, _languageRepository, cancellationToken);

        var color = new Color(request.HexCode, request.RgbCode, request.IsActive);

        foreach (var t in request.Translations)
        {
            color.AddOrUpdateTranslation(t.LanguageCode, t.Name, t.Description);
        }

        await _repository.AddAsync(color, cancellationToken);
        return color.Id;
    }

    private static void ValidateColorCodes(string hexCode, string rgbCode)
    {
        var errors = new Dictionary<string, string[]>();
        if (string.IsNullOrWhiteSpace(hexCode)) errors["hexCode"] = ["HexCode is required"];
        if (string.IsNullOrWhiteSpace(rgbCode)) errors["rgbCode"] = ["RgbCode is required"];
        if (errors.Count > 0) throw new ValidationException(errors);
    }
}

public sealed class UpdateColorHandler : IRequestHandler<UpdateColorCommand>
{
    private readonly IColorRepository _repository;
    private readonly ILanguageRepository _languageRepository;

    public UpdateColorHandler(IColorRepository repository, ILanguageRepository languageRepository)
    {
        _repository = repository;
        _languageRepository = languageRepository;
    }

    public async Task Handle(UpdateColorCommand request, CancellationToken cancellationToken)
    {
        await TranslationValidation.EnsureValidAsync(
            request.Translations, t => t.LanguageCode, t => t.Name, _languageRepository, cancellationToken);

        var color = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Color", request.Id);

        color.Update(request.HexCode, request.RgbCode, request.IsActive);

        foreach (var t in request.Translations)
        {
            color.AddOrUpdateTranslation(t.LanguageCode, t.Name, t.Description);
        }

        await _repository.UpdateAsync(color, cancellationToken);
    }
}

public sealed class DeleteColorHandler : IRequestHandler<DeleteColorCommand>
{
    private readonly IColorRepository _repository;

    public DeleteColorHandler(IColorRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteColorCommand request, CancellationToken cancellationToken)
    {
        var color = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Color", request.Id);

        await _repository.DeleteAsync(color, cancellationToken);
    }
}
