using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Entities;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.Settings.Attributes.Occasions;

public sealed record CreateOccasionCommand(bool IsActive, List<OccasionTranslationInput> Translations) : IRequest<Guid>;

public sealed record UpdateOccasionCommand(Guid Id, bool IsActive, List<OccasionTranslationInput> Translations) : IRequest;

public sealed record DeleteOccasionCommand(Guid Id) : IRequest;

public sealed class CreateOccasionHandler : IRequestHandler<CreateOccasionCommand, Guid>
{
    private readonly IOccasionRepository _repository;
    private readonly ILanguageRepository _languageRepository;

    public CreateOccasionHandler(IOccasionRepository repository, ILanguageRepository languageRepository)
    {
        _repository = repository;
        _languageRepository = languageRepository;
    }

    public async Task<Guid> Handle(CreateOccasionCommand request, CancellationToken cancellationToken)
    {
        await TranslationValidation.EnsureValidAsync(
            request.Translations, t => t.LanguageCode, t => t.Name, _languageRepository, cancellationToken);

        var occasion = new Occasion(request.IsActive);

        foreach (var t in request.Translations)
        {
            occasion.AddOrUpdateTranslation(t.LanguageCode, t.Name, t.Description);
        }

        await _repository.AddAsync(occasion, cancellationToken);
        return occasion.Id;
    }
}

public sealed class UpdateOccasionHandler : IRequestHandler<UpdateOccasionCommand>
{
    private readonly IOccasionRepository _repository;
    private readonly ILanguageRepository _languageRepository;

    public UpdateOccasionHandler(IOccasionRepository repository, ILanguageRepository languageRepository)
    {
        _repository = repository;
        _languageRepository = languageRepository;
    }

    public async Task Handle(UpdateOccasionCommand request, CancellationToken cancellationToken)
    {
        await TranslationValidation.EnsureValidAsync(
            request.Translations, t => t.LanguageCode, t => t.Name, _languageRepository, cancellationToken);

        var occasion = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Occasion", request.Id);

        occasion.SetActive(request.IsActive);

        foreach (var t in request.Translations)
        {
            occasion.AddOrUpdateTranslation(t.LanguageCode, t.Name, t.Description);
        }

        await _repository.UpdateAsync(occasion, cancellationToken);
    }
}

public sealed class DeleteOccasionHandler : IRequestHandler<DeleteOccasionCommand>
{
    private readonly IOccasionRepository _repository;

    public DeleteOccasionHandler(IOccasionRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteOccasionCommand request, CancellationToken cancellationToken)
    {
        var occasion = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Occasion", request.Id);

        await _repository.DeleteAsync(occasion, cancellationToken);
    }
}
