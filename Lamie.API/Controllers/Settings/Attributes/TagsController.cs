using Lamie.Application.MasterData.Tags;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lamie.API.Controllers;

[ApiController]
[Route("api/settings/attributes/tags")]
public sealed class TagsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TagsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetTags(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllTagsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTagById(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTagByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTag([FromBody] CreateTagCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetTagById), new { id }, new { id });
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTag([FromBody] UpdateTagCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTag(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteTagCommand(id), cancellationToken);
        return NoContent();
    }
}

