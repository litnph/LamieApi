using Lamie.Domain.Entities;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.MasterData.Tags;

public sealed class GetAllTagsHandler : IRequestHandler<GetAllTagsQuery, List<Tag>>
{
    private readonly ITagRepository _repository;

    public GetAllTagsHandler(ITagRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Tag>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        var tags = await _repository.GetAllAsync();
        return tags;
    }
}

