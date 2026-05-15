using FluentValidation;
using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Entities.Orders;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.Orders.Commands;

public sealed record ChangeOrderStatusCommand(Guid Id, OrderStatus Status) : IRequest;

public sealed class ChangeOrderStatusValidator : AbstractValidator<ChangeOrderStatusCommand>
{
    public ChangeOrderStatusValidator()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty);
        RuleFor(x => x.Status).IsInEnum();
    }
}

public sealed class ChangeOrderStatusHandler : IRequestHandler<ChangeOrderStatusCommand>
{
    private readonly IOrderRepository _repository;

    public ChangeOrderStatusHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(ChangeOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Order", request.Id);

        order.ChangeStatus(request.Status);
        await _repository.UpdateAsync(order, cancellationToken);
    }
}

public sealed record ChangePaymentStatusCommand(Guid Id, PaymentStatus Status) : IRequest;

public sealed class ChangePaymentStatusValidator : AbstractValidator<ChangePaymentStatusCommand>
{
    public ChangePaymentStatusValidator()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty);
        RuleFor(x => x.Status).IsInEnum();
    }
}

public sealed class ChangePaymentStatusHandler : IRequestHandler<ChangePaymentStatusCommand>
{
    private readonly IOrderRepository _repository;

    public ChangePaymentStatusHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(ChangePaymentStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Order", request.Id);

        order.ChangePaymentStatus(request.Status);
        await _repository.UpdateAsync(order, cancellationToken);
    }
}
