using Lamie.Application.Settings.Attributes.Channels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lamie.API.Controllers;

[ApiController]
[Authorize]
[Route("api/settings/attributes/channels")]
public sealed class ChannelsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChannelsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetChannels(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllChannelsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetChannelById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetChannelByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "ManagerOrAbove")]
    public async Task<IActionResult> CreateChannel([FromBody] CreateChannelCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetChannelById), new { id }, new { id });
    }

    [HttpPut]
    [Authorize(Policy = "ManagerOrAbove")]
    public async Task<IActionResult> UpdateChannel([FromBody] UpdateChannelCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteChannel(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteChannelCommand(id), cancellationToken);
        return NoContent();
    }
}
