using Lamie.Application.Settings.Attributes.Occasions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lamie.API.Controllers;

[ApiController]
[Authorize]
[Route("api/settings/attributes/occasions")]
public sealed class OccasionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public OccasionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetOccasions(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllOccasionsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOccasionById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetOccasionByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "ManagerOrAbove")]
    public async Task<IActionResult> CreateOccasion([FromBody] CreateOccasionCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetOccasionById), new { id }, new { id });
    }

    [HttpPut]
    [Authorize(Policy = "ManagerOrAbove")]
    public async Task<IActionResult> UpdateOccasion([FromBody] UpdateOccasionCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteOccasion(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteOccasionCommand(id), cancellationToken);
        return NoContent();
    }
}
