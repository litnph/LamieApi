using Lamie.Domain.Entities;
using MediatR;

namespace Lamie.Application.MasterData.Tags;

public sealed record GetAllTagsQuery() : IRequest<List<Tag>>;

