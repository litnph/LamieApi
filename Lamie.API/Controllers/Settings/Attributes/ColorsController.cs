using Lamie.Application.Settings.Attributes.Colors;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lamie.API.Controllers;

[ApiController]
[Authorize]
[Route("api/settings/attributes/colors")]
public sealed class ColorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ColorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetColors(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllColorsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetColorById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetColorByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "ManagerOrAbove")]
    public async Task<IActionResult> CreateColor([FromBody] CreateColorCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetColorById), new { id }, new { id });
    }

    [HttpPut]
    [Authorize(Policy = "ManagerOrAbove")]
    public async Task<IActionResult> UpdateColor([FromBody] UpdateColorCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteColor(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteColorCommand(id), cancellationToken);
        return NoContent();
    }
}
