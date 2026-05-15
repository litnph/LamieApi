using Lamie.Application.Common.Exceptions;
using Lamie.Application.Orders.Dtos;
using Lamie.Domain.Entities.Orders;
using Lamie.Domain.Repositories;
using Lamie.Shared.Common;
using MediatR;

namespace Lamie.Application.Orders.Queries;

public sealed record GetOrderByIdQuery(Guid Id) : IRequest<OrderDetailDto>;

public sealed class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderDetailDto>
{
    private readonly IOrderRepository _repository;

    public GetOrderByIdHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<OrderDetailDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Order", request.Id);

        var logs = await _repository.GetChangeLogsAsync(order.Id, cancellationToken);
        return OrderMapper.ToDetail(order, logs);
    }
}

public sealed record ListOrdersQuery(OrderListFilter Filter) : IRequest<PagedResult<OrderListItemDto>>;

public sealed class ListOrdersHandler : IRequestHandler<ListOrdersQuery, PagedResult<OrderListItemDto>>
{
    private readonly IOrderRepository _repository;

    public ListOrdersHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<OrderListItemDto>> Handle(ListOrdersQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter with
        {
            Page = request.Filter.Page <= 0 ? 1 : request.Filter.Page,
            PageSize = request.Filter.PageSize is <= 0 or > 200 ? 20 : request.Filter.PageSize,
        };

        var (items, total) = await _repository.ListAsync(filter, cancellationToken);
        var dtos = items.Select(OrderMapper.ToListItem).ToList();
        return new PagedResult<OrderListItemDto>(dtos, total, filter.Page, filter.PageSize);
    }
}

public sealed record GetCalendarOrdersQuery(DateOnly Date) : IRequest<List<OrderCalendarItemDto>>;

public sealed class GetCalendarOrdersHandler : IRequestHandler<GetCalendarOrdersQuery, List<OrderCalendarItemDto>>
{
    private readonly IOrderRepository _repository;

    public GetCalendarOrdersHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<OrderCalendarItemDto>> Handle(GetCalendarOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _repository.GetByDeliveryDateAsync(request.Date, cancellationToken);
        return orders
            .OrderBy(o => o.DeliveryAt)
            .Select(OrderMapper.ToCalendarItem)
            .ToList();
    }
}

public sealed record GetDeliveryLocationsQuery(DateOnly Date) : IRequest<List<OrderDeliveryLocationDto>>;

public sealed class GetDeliveryLocationsHandler : IRequestHandler<GetDeliveryLocationsQuery, List<OrderDeliveryLocationDto>>
{
    private readonly IOrderRepository _repository;

    public GetDeliveryLocationsHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<OrderDeliveryLocationDto>> Handle(GetDeliveryLocationsQuery request, CancellationToken cancellationToken)
    {
        var orders = await _repository.GetByDeliveryDateAsync(request.Date, cancellationToken);
        return orders
            .Where(o => !o.PickupAtShop && o.DeliveryLatitude.HasValue && o.DeliveryLongitude.HasValue)
            .OrderBy(o => o.DeliveryAt)
            .Select(o => new OrderDeliveryLocationDto
            {
                Id = o.Id,
                OrderCode = o.OrderCode,
                RecipientName = o.RecipientName,
                DeliveryAddress = o.DeliveryAddress,
                Latitude = o.DeliveryLatitude!.Value,
                Longitude = o.DeliveryLongitude!.Value,
                DeliveryAt = o.DeliveryAt,
                OrderStatus = o.OrderStatus,
            })
            .ToList();
    }
}
