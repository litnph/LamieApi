using Lamie.Application.Settings.Attributes.Collections;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lamie.API.Controllers;

[ApiController]
[Authorize]
[Route("api/settings/attributes/collections")]
public sealed class CollectionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CollectionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetCollections(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllCollectionsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCollectionById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCollectionByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "ManagerOrAbove")]
    public async Task<IActionResult> CreateCollection([FromBody] CreateCollectionCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetCollectionById), new { id }, new { id });
    }

    [HttpPut]
    [Authorize(Policy = "ManagerOrAbove")]
    public async Task<IActionResult> UpdateCollection([FromBody] UpdateCollectionCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteCollection(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCollectionCommand(id), cancellationToken);
        return NoContent();
    }
}
