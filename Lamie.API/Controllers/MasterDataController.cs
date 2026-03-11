using Lamie.Application.MasterData.Tags;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lamie.API.Controllers;

[ApiController]
[Route("api/masterdata")]
public sealed class MasterDataController : ControllerBase
{
    private readonly IMediator _mediator;

    public MasterDataController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // TAGS
    [HttpGet("tags")]
    public async Task<IActionResult> GetTags(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllTagsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost("tags")]
    public async Task<IActionResult> CreateTag([FromBody] CreateTagCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetTags), new { id }, new { id });
    }

    [HttpPut("tags/{id:int}")]
    public async Task<IActionResult> UpdateTag(int id, [FromBody] UpdateTagCommand body, CancellationToken cancellationToken)
    {
        var command = body with { Id = id };
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("tags/{id:int}")]
    public async Task<IActionResult> DeleteTag(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteTagCommand(id), cancellationToken);
        return NoContent();
    }
}

