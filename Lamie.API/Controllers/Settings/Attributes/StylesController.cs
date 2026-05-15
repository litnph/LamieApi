using Lamie.Application.Settings.Attributes.Styles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lamie.API.Controllers;

[ApiController]
[Authorize]
[Route("api/settings/attributes/styles")]
public sealed class StylesController : ControllerBase
{
    private readonly IMediator _mediator;

    public StylesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetStyles(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllStylesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetStyleById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetStyleByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "ManagerOrAbove")]
    public async Task<IActionResult> CreateStyle([FromBody] CreateStyleCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetStyleById), new { id }, new { id });
    }

    [HttpPut]
    [Authorize(Policy = "ManagerOrAbove")]
    public async Task<IActionResult> UpdateStyle([FromBody] UpdateStyleCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteStyle(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteStyleCommand(id), cancellationToken);
        return NoContent();
    }
}
