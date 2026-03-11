using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Entities;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.MasterData.Tags;

public sealed class CreateTagHandler : IRequestHandler<CreateTagCommand, int>
{
    private readonly ITagRepository _repository;

    public CreateTagHandler(ITagRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var tag = new Tag(request.IsActive);

        foreach (var t in request.Translations)
        {
            tag.AddOrUpdateTranslation(t.LanguageCode, t.Name, t.Description);
        }

        await _repository.AddAsync(tag);
        return tag.Id;
    }
}

public sealed class UpdateTagHandler : IRequestHandler<UpdateTagCommand>
{
    private readonly ITagRepository _repository;

    public UpdateTagHandler(ITagRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _repository.GetByIdAsync(request.Id);
        if (tag is null)
        {
            throw new NotFoundException("Tag", request.Id);
        }

        tag.SetActive(request.IsActive);

        foreach (var t in request.Translations)
        {
            tag.AddOrUpdateTranslation(t.LanguageCode, t.Name, t.Description);
        }

        await _repository.UpdateAsync(tag);
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
        var tag = await _repository.GetByIdAsync(request.Id);
        if (tag is null)
        {
            throw new NotFoundException("Tag", request.Id);
        }

        await _repository.DeleteAsync(tag);
    }
}

