using Lamie.Application.Settings.Products.Commands;
using Lamie.Application.Settings.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lamie.API.Controllers;

[ApiController]
[Authorize]
[Route("api/settings/products")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [Authorize(Policy = "ManagerOrAbove")]
    public async Task<IActionResult> Create([FromForm] CreateProductCommand command, CancellationToken cancellationToken)
    {
        var productId = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = productId }, new { id = productId });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllProductsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPut]
    [Consumes("multipart/form-data")]
    [Authorize(Policy = "ManagerOrAbove")]
    public async Task<IActionResult> Update([FromForm] UpdateProductCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteProductCommand(id), cancellationToken);
        return NoContent();
    }
}
