using MediatR;

namespace Lamie.Application.Settings.Products.Commands;

public sealed record DeleteProductCommand(Guid Id) : IRequest;
