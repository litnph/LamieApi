using AutoMapper;
using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.Settings.Attributes.Tags;

public sealed record GetAllTagsQuery() : IRequest<List<TagDto>>;

public sealed record GetTagByIdQuery(Guid Id) : IRequest<TagDto>;

public sealed class GetAllTagsHandler : IRequestHandler<GetAllTagsQuery, List<TagDto>>
{
    private readonly ITagRepository _repository;
    private readonly IMapper _mapper;

    public GetAllTagsHandler(ITagRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<TagDto>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        var tags = await _repository.GetAllAsync(cancellationToken);
        return tags.Select(_mapper.Map<TagDto>).ToList();
    }
}

public sealed class GetTagByIdHandler : IRequestHandler<GetTagByIdQuery, TagDto>
{
    private readonly ITagRepository _repository;
    private readonly IMapper _mapper;

    public GetTagByIdHandler(ITagRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TagDto> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
    {
        var tag = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Tag", request.Id);

        return _mapper.Map<TagDto>(tag);
    }
}
