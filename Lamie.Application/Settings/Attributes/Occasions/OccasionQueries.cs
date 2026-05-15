using AutoMapper;
using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.Settings.Attributes.Occasions;

public sealed record GetAllOccasionsQuery() : IRequest<List<OccasionDto>>;

public sealed record GetOccasionByIdQuery(Guid Id) : IRequest<OccasionDto>;

public sealed class GetAllOccasionsHandler : IRequestHandler<GetAllOccasionsQuery, List<OccasionDto>>
{
    private readonly IOccasionRepository _repository;
    private readonly IMapper _mapper;

    public GetAllOccasionsHandler(IOccasionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<OccasionDto>> Handle(GetAllOccasionsQuery request, CancellationToken cancellationToken)
    {
        var occasions = await _repository.GetAllAsync(cancellationToken);
        return occasions.Select(_mapper.Map<OccasionDto>).ToList();
    }
}

public sealed class GetOccasionByIdHandler : IRequestHandler<GetOccasionByIdQuery, OccasionDto>
{
    private readonly IOccasionRepository _repository;
    private readonly IMapper _mapper;

    public GetOccasionByIdHandler(IOccasionRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<OccasionDto> Handle(GetOccasionByIdQuery request, CancellationToken cancellationToken)
    {
        var occasion = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Occasion", request.Id);

        return _mapper.Map<OccasionDto>(occasion);
    }
}
