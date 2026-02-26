using Lamie.Application.Products.Commands;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Lamie.API.Controllers
{
    [ApiController]
    [Route("api/admin/products")]
    public class AdminProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Admin: Tạo sản phẩm mới
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
        {
            var productId = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetProductById),
                new { id = productId },
                new { id = productId }
            );
        }

        /// <summary>
        /// Admin: Lấy chi tiết sản phẩm (ví dụ minh họa)
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductById(int id, CancellationToken cancellationToken)
        {
            // Ví dụ: bạn sẽ có GetProductByIdQuery
            // var result = await _mediator.Send(new GetProductByIdQuery(id));

            return Ok(new { id });
        }
    }
}
