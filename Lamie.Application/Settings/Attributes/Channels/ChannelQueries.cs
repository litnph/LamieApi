using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.Settings.Attributes.Channels;

public sealed record GetAllChannelsQuery() : IRequest<List<ChannelDto>>;

public sealed record GetChannelByIdQuery(Guid Id) : IRequest<ChannelDto>;

public sealed class GetAllChannelsHandler : IRequestHandler<GetAllChannelsQuery, List<ChannelDto>>
{
    private readonly IChannelRepository _repository;

    public GetAllChannelsHandler(IChannelRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ChannelDto>> Handle(GetAllChannelsQuery request, CancellationToken cancellationToken)
    {
        var channels = await _repository.GetAllAsync(cancellationToken);
        return channels.Select(c => new ChannelDto
        {
            Id = c.Id,
            Code = c.Code,
            Name = c.Name,
            IconUrl = c.IconUrl,
            IsActive = c.IsActive,
            SortOrder = c.SortOrder,
        }).ToList();
    }
}

public sealed class GetChannelByIdHandler : IRequestHandler<GetChannelByIdQuery, ChannelDto>
{
    private readonly IChannelRepository _repository;

    public GetChannelByIdHandler(IChannelRepository repository)
    {
        _repository = repository;
    }

    public async Task<ChannelDto> Handle(GetChannelByIdQuery request, CancellationToken cancellationToken)
    {
        var c = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Channel", request.Id);

        return new ChannelDto
        {
            Id = c.Id,
            Code = c.Code,
            Name = c.Name,
            IconUrl = c.IconUrl,
            IsActive = c.IsActive,
            SortOrder = c.SortOrder,
        };
    }
}
