using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.MasterData.Tags;

public sealed class GetTagByIdHandler : IRequestHandler<GetTagByIdQuery, TagDto>
{
    private readonly ITagRepository _repository;
    private static readonly TagMapper _mapper = new();

    public GetTagByIdHandler(ITagRepository repository)
    {
        _repository = repository;
    }

    public async Task<TagDto> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
    {
        var tag = await _repository.GetByIdAsync(request.Id);
        if (tag is null)
        {
            throw new NotFoundException("Tag", request.Id);
        }

        return _mapper.ToDto(tag);
    }
}

