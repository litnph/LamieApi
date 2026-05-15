using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.Orders.Commands;

public sealed record DeleteOrderCommand(Guid Id) : IRequest;

public sealed class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IOrderRepository _repository;

    public DeleteOrderHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Order", request.Id);

        await _repository.DeleteAsync(order, cancellationToken);
    }
}
