using Lamie.Application.Settings.Attributes.Categories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lamie.API.Controllers;

[ApiController]
[Authorize]
[Route("api/settings/attributes/categories")]
public sealed class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllCategoriesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCategoryById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "ManagerOrAbove")]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetCategoryById), new { id }, new { id });
    }

    [HttpPut]
    [Authorize(Policy = "ManagerOrAbove")]
    public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCategoryCommand(id), cancellationToken);
        return NoContent();
    }
}
