using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Entities;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.Settings.Attributes.Collections;

public sealed record CreateCollectionCommand(bool IsActive, List<CollectionTranslationInput> Translations) : IRequest<Guid>;

public sealed record UpdateCollectionCommand(Guid Id, bool IsActive, List<CollectionTranslationInput> Translations) : IRequest;

public sealed record DeleteCollectionCommand(Guid Id) : IRequest;

public sealed class CreateCollectionHandler : IRequestHandler<CreateCollectionCommand, Guid>
{
    private readonly ICollectionRepository _repository;
    private readonly ILanguageRepository _languageRepository;

    public CreateCollectionHandler(ICollectionRepository repository, ILanguageRepository languageRepository)
    {
        _repository = repository;
        _languageRepository = languageRepository;
    }

    public async Task<Guid> Handle(CreateCollectionCommand request, CancellationToken cancellationToken)
    {
        await TranslationValidation.EnsureValidAsync(
            request.Translations, t => t.LanguageCode, t => t.Name, _languageRepository, cancellationToken);

        var collection = new Collection(request.IsActive);

        foreach (var t in request.Translations)
        {
            collection.AddOrUpdateTranslation(t.LanguageCode, t.Name, t.Description);
        }

        await _repository.AddAsync(collection, cancellationToken);
        return collection.Id;
    }
}

public sealed class UpdateCollectionHandler : IRequestHandler<UpdateCollectionCommand>
{
    private readonly ICollectionRepository _repository;
    private readonly ILanguageRepository _languageRepository;

    public UpdateCollectionHandler(ICollectionRepository repository, ILanguageRepository languageRepository)
    {
        _repository = repository;
        _languageRepository = languageRepository;
    }

    public async Task Handle(UpdateCollectionCommand request, CancellationToken cancellationToken)
    {
        await TranslationValidation.EnsureValidAsync(
            request.Translations, t => t.LanguageCode, t => t.Name, _languageRepository, cancellationToken);

        var collection = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Collection", request.Id);

        collection.SetActive(request.IsActive);

        foreach (var t in request.Translations)
        {
            collection.AddOrUpdateTranslation(t.LanguageCode, t.Name, t.Description);
        }

        await _repository.UpdateAsync(collection, cancellationToken);
    }
}

public sealed class DeleteCollectionHandler : IRequestHandler<DeleteCollectionCommand>
{
    private readonly ICollectionRepository _repository;

    public DeleteCollectionHandler(ICollectionRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteCollectionCommand request, CancellationToken cancellationToken)
    {
        var collection = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Collection", request.Id);

        await _repository.DeleteAsync(collection, cancellationToken);
    }
}
