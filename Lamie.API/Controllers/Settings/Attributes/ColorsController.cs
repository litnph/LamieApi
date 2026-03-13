using Lamie.Application.Settings.Attributes.Colors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lamie.API.Controllers;

[ApiController]
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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetColorById(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetColorByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateColor([FromBody] CreateColorCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetColorById), new { id }, new { id });
    }

    [HttpPut]
    public async Task<IActionResult> UpdateColor([FromBody] UpdateColorCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteColor(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteColorCommand(id), cancellationToken);
        return NoContent();
    }
}

