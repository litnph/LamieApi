using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Entities;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.Settings.Attributes.Tags;

public sealed record CreateTagCommand(bool IsActive, List<TagTranslationInput> Translations) : IRequest<Guid>;

public sealed record UpdateTagCommand(Guid Id, bool IsActive, List<TagTranslationInput> Translations) : IRequest;

public sealed record DeleteTagCommand(Guid Id) : IRequest;

public sealed class CreateTagHandler : IRequestHandler<CreateTagCommand, Guid>
{
    private readonly ITagRepository _repository;
    private readonly ILanguageRepository _languageRepository;

    public CreateTagHandler(ITagRepository repository, ILanguageRepository languageRepository)
    {
        _repository = repository;
        _languageRepository = languageRepository;
    }

    public async Task<Guid> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        await TranslationValidation.EnsureValidAsync(
            request.Translations, t => t.LanguageCode, t => t.Name, _languageRepository, cancellationToken);

        var tag = new Tag(request.IsActive);

        foreach (var t in request.Translations)
        {
            tag.AddOrUpdateTranslation(t.LanguageCode, t.Name, t.Description);
        }

        await _repository.AddAsync(tag, cancellationToken);
        return tag.Id;
    }
}

public sealed class UpdateTagHandler : IRequestHandler<UpdateTagCommand>
{
    private readonly ITagRepository _repository;
    private readonly ILanguageRepository _languageRepository;

    public UpdateTagHandler(ITagRepository repository, ILanguageRepository languageRepository)
    {
        _repository = repository;
        _languageRepository = languageRepository;
    }

    public async Task Handle(UpdateTagCommand request, CancellationToken cancellationToken)
    {
        await TranslationValidation.EnsureValidAsync(
            request.Translations, t => t.LanguageCode, t => t.Name, _languageRepository, cancellationToken);

        var tag = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Tag", request.Id);

        tag.SetActive(request.IsActive);

        foreach (var t in request.Translations)
        {
            tag.AddOrUpdateTranslation(t.LanguageCode, t.Name, t.Description);
        }

        await _repository.UpdateAsync(tag, cancellationToken);
    }
}

public sealed class DeleteTagHandler : IRequestHandler<DeleteTagCommand>
{
    private readonly ITagRepository _repository;

    public DeleteTagHandler(ITagRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Tag", request.Id);

        await _repository.DeleteAsync(tag, cancellationToken);
    }
}
