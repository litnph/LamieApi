using Lamie.Application.MasterData.Tags;
using Lamie.API.Models.MasterData;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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

    [HttpGet("tags/{id:int}")]
    public async Task<IActionResult> GetTagById(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTagByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost("tags")]
    public async Task<IActionResult> CreateTag([FromBody] CreateTagRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateTagCommand(
            request.IsActive,
            request.Translations.Select(t => new TagTranslationInput(t.LanguageCode, t.Name, t.Description)).ToList()
        );

        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetTagById), new { id }, new { id });
    }

    [HttpPut("tags/{id:int}")]
    public async Task<IActionResult> UpdateTag(int id, [FromBody] UpdateTagRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateTagCommand(
            id,
            request.IsActive,
            request.Translations.Select(t => new TagTranslationInput(t.LanguageCode, t.Name, t.Description)).ToList()
        );

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

