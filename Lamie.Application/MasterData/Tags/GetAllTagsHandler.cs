using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.MasterData.Tags;

public sealed class GetAllTagsHandler : IRequestHandler<GetAllTagsQuery, List<TagDto>>
{
    private readonly ITagRepository _repository;
    private static readonly TagMapper _mapper = new();

    public GetAllTagsHandler(ITagRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<TagDto>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        var tags = await _repository.GetAllAsync();
        return tags.Select(_mapper.ToDto).ToList();
    }
}

