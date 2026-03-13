using Lamie.Application.Settings.Products.Commands;
using Lamie.Application.Settings.Products.Dtos;
using Lamie.Application.Settings.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lamie.API.Controllers
{
    [ApiController]
    [Route("api/settings/products")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Admin: Tạo sản phẩm mới
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
        {
            var productId = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = productId },
                new { id = productId }
            );
        }

        /// <summary>
        /// Admin: Lấy chi tiết sản phẩm (ví dụ minh họa)
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));
            return Ok(result);
        }

        /// <summary>
        /// Get All
        /// </summary>
        [HttpGet()]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllProductsQuery());
            return Ok(result);
        }

        /// <summary>
        /// Admin: Cập nhật sản phẩm
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Admin: Xóa sản phẩm
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteProductCommand(id), cancellationToken);
            return NoContent();
        }

        // Logic sanitize / build path đã được chuyển xuống Application layer (CreateProductHandler)
    }
}
