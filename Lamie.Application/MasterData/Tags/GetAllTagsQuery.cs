using MediatR;

namespace Lamie.Application.MasterData.Tags;

public sealed record GetAllTagsQuery() : IRequest<List<TagDto>>;

public sealed record GetTagByIdQuery(int Id) : IRequest<TagDto>;

