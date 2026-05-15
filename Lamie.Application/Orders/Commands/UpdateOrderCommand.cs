using FluentValidation;
using Lamie.Application.Common.Exceptions;
using ValidationException = Lamie.Application.Common.Exceptions.ValidationException;
using Lamie.Application.Common.Storage;
using Lamie.Application.Orders.Dtos;
using Lamie.Domain.Entities.Orders;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.Orders.Commands;

public sealed class UpdateOrderItemInput
{
    public Guid? Id { get; set; }
    public Guid? ProductId { get; set; }
    public string? ProductSku { get; set; }
    public string ProductName { get; set; } = default!;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public string? Note { get; set; }
}

public sealed class UpdateOrderCommand : IRequest<OrderDetailDto>
{
    public Guid Id { get; set; }
    public string OrdererName { get; set; } = default!;
    public string OrdererPhone { get; set; } = default!;
    public Guid ChannelId { get; set; }
    public string RecipientName { get; set; } = default!;
    public string RecipientPhone { get; set; } = default!;
    public bool PickupAtShop { get; set; }
    public string? DeliveryAddress { get; set; }
    public double? DeliveryLatitude { get; set; }
    public double? DeliveryLongitude { get; set; }
    public DateTime DeliveryAt { get; set; }
    public decimal DepositAmount { get; set; }
    public decimal ShippingFee { get; set; }
    public decimal? ShippingFeeActual { get; set; }
    public string? Description { get; set; }
    public string? ContentNote { get; set; }
    public List<UpdateOrderItemInput> Items { get; set; } = new();
}

public sealed class UpdateOrderValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderValidator()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty);
        RuleFor(x => x.OrdererName).NotEmpty();
        RuleFor(x => x.OrdererPhone).NotEmpty();
        RuleFor(x => x.ChannelId).NotEqual(Guid.Empty);
        RuleFor(x => x.RecipientName).NotEmpty();
        RuleFor(x => x.RecipientPhone).NotEmpty();
        RuleFor(x => x.DeliveryAt).NotEqual(default(DateTime));
        RuleFor(x => x.DepositAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ShippingFee).GreaterThanOrEqualTo(0);

        When(x => !x.PickupAtShop, () =>
        {
            RuleFor(x => x.DeliveryAddress).NotEmpty();
        });

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.ProductName).NotEmpty();
            item.RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
            item.RuleFor(x => x.Quantity).GreaterThan(0);
        });
    }
}

public sealed class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, OrderDetailDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IChannelRepository _channelRepository;

    public UpdateOrderHandler(IOrderRepository orderRepository, IChannelRepository channelRepository)
    {
        _orderRepository = orderRepository;
        _channelRepository = channelRepository;
    }

    public async Task<OrderDetailDto> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException("Order", command.Id);

        var channel = await _channelRepository.GetByIdAsync(command.ChannelId, cancellationToken)
            ?? throw new ValidationException(new() { ["channelId"] = ["Channel does not exist"] });

        order.UpdateBasicInfo(
            command.OrdererName,
            command.OrdererPhone,
            channel.Id,
            command.RecipientName,
            command.RecipientPhone,
            command.DeliveryAt,
            command.DepositAmount,
            command.Description,
            command.ContentNote);

        order.MarkPickupAtShop(command.PickupAtShop);
        order.SetDeliveryLocation(command.DeliveryAddress, command.DeliveryLatitude, command.DeliveryLongitude);
        order.ChangeShippingFee(command.ShippingFee, command.ShippingFeeActual);

        SyncItems(order, command.Items);

        await _orderRepository.UpdateAsync(order, cancellationToken);

        var logs = await _orderRepository.GetChangeLogsAsync(order.Id, cancellationToken);
        return OrderMapper.ToDetail(order, logs);
    }

    private static void SyncItems(Order order, List<UpdateOrderItemInput> incoming)
    {
        var existing = order.Items.ToDictionary(i => i.Id);
        var keepIds = incoming.Where(i => i.Id.HasValue).Select(i => i.Id!.Value).ToHashSet();

        foreach (var id in existing.Keys.Except(keepIds))
        {
            order.RemoveItem(id);
        }

        foreach (var input in incoming)
        {
            if (input.Id.HasValue && existing.ContainsKey(input.Id.Value))
            {
                order.UpdateItem(input.Id.Value, input.UnitPrice, input.Quantity, input.Note);
            }
            else
            {
                order.AddItem(input.ProductId, input.ProductSku, input.ProductName, input.UnitPrice, input.Quantity, input.Note);
            }
        }
    }
}
