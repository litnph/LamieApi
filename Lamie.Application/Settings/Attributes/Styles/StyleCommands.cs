using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Entities;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.Settings.Attributes.Styles;

public sealed record CreateStyleCommand(bool IsActive, List<StyleTranslationInput> Translations) : IRequest<Guid>;

public sealed record UpdateStyleCommand(Guid Id, bool IsActive, List<StyleTranslationInput> Translations) : IRequest;

public sealed record DeleteStyleCommand(Guid Id) : IRequest;

public sealed class CreateStyleHandler : IRequestHandler<CreateStyleCommand, Guid>
{
    private readonly IStyleRepository _repository;
    private readonly ILanguageRepository _languageRepository;

    public CreateStyleHandler(IStyleRepository repository, ILanguageRepository languageRepository)
    {
        _repository = repository;
        _languageRepository = languageRepository;
    }

    public async Task<Guid> Handle(CreateStyleCommand request, CancellationToken cancellationToken)
    {
        await TranslationValidation.EnsureValidAsync(
            request.Translations, t => t.LanguageCode, t => t.Name, _languageRepository, cancellationToken);

        var style = new Style(request.IsActive);

        foreach (var t in request.Translations)
        {
            style.AddOrUpdateTranslation(t.LanguageCode, t.Name, t.Description);
        }

        await _repository.AddAsync(style, cancellationToken);
        return style.Id;
    }
}

public sealed class UpdateStyleHandler : IRequestHandler<UpdateStyleCommand>
{
    private readonly IStyleRepository _repository;
    private readonly ILanguageRepository _languageRepository;

    public UpdateStyleHandler(IStyleRepository repository, ILanguageRepository languageRepository)
    {
        _repository = repository;
        _languageRepository = languageRepository;
    }

    public async Task Handle(UpdateStyleCommand request, CancellationToken cancellationToken)
    {
        await TranslationValidation.EnsureValidAsync(
            request.Translations, t => t.LanguageCode, t => t.Name, _languageRepository, cancellationToken);

        var style = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Style", request.Id);

        style.SetActive(request.IsActive);

        foreach (var t in request.Translations)
        {
            style.AddOrUpdateTranslation(t.LanguageCode, t.Name, t.Description);
        }

        await _repository.UpdateAsync(style, cancellationToken);
    }
}

public sealed class DeleteStyleHandler : IRequestHandler<DeleteStyleCommand>
{
    private readonly IStyleRepository _repository;

    public DeleteStyleHandler(IStyleRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteStyleCommand request, CancellationToken cancellationToken)
    {
        var style = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Style", request.Id);

        await _repository.DeleteAsync(style, cancellationToken);
    }
}
