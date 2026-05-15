using Lamie.Application.Orders.Commands;
using Lamie.Application.Orders.Queries;
using Lamie.Domain.Entities.Orders;
using Lamie.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lamie.API.Controllers;

[ApiController]
[Authorize]
[Route("api/orders")]
public sealed class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm] CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        if (command.Id != id)
        {
            return BadRequest(new { success = false, message = "Route id does not match command id." });
        }

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] OrderListQueryParams query, CancellationToken cancellationToken)
    {
        var filter = new OrderListFilter
        {
            OrderStatus = query.OrderStatus,
            PaymentStatus = query.PaymentStatus,
            ChannelId = query.ChannelId,
            DeliveryFrom = query.DeliveryFrom,
            DeliveryTo = query.DeliveryTo,
            CreatedFrom = query.CreatedFrom,
            CreatedTo = query.CreatedTo,
            Phone = query.Phone,
            Search = query.Search,
            Page = query.Page ?? 1,
            PageSize = query.PageSize ?? 20,
        };

        var result = await _mediator.Send(new ListOrdersQuery(filter), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetOrderByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeStatusRequest body, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ChangeOrderStatusCommand(id, body.Status), cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:guid}/payment-status")]
    public async Task<IActionResult> ChangePaymentStatus(Guid id, [FromBody] ChangePaymentStatusRequest body, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ChangePaymentStatusCommand(id, body.Status), cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "ManagerOrAbove")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteOrderCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpGet("calendar")]
    public async Task<IActionResult> Calendar([FromQuery] DateOnly date, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCalendarOrdersQuery(date), cancellationToken);
        return Ok(result);
    }

    [HttpGet("calendar/locations")]
    public async Task<IActionResult> CalendarLocations([FromQuery] DateOnly date, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDeliveryLocationsQuery(date), cancellationToken);
        return Ok(result);
    }
}

public sealed class OrderListQueryParams
{
    public OrderStatus? OrderStatus { get; set; }
    public PaymentStatus? PaymentStatus { get; set; }
    public Guid? ChannelId { get; set; }
    public DateTime? DeliveryFrom { get; set; }
    public DateTime? DeliveryTo { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
    public string? Phone { get; set; }
    public string? Search { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}

public sealed record ChangeStatusRequest(OrderStatus Status);
public sealed record ChangePaymentStatusRequest(PaymentStatus Status);
