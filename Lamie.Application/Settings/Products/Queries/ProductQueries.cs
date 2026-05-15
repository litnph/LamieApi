using Lamie.Application.Settings.Products.Dtos;
using MediatR;

namespace Lamie.Application.Settings.Products.Queries;

public sealed record GetAllProductsQuery() : IRequest<List<ProductDetailsDto>>;

public sealed record GetProductByIdQuery(Guid Id) : IRequest<ProductDetailsDto>;
