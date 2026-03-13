using Lamie.API.Models;
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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateProductMultipartRequest request, CancellationToken cancellationToken)
        {
            // Payload là JSON của CreateProductCommand
            var command = System.Text.Json.JsonSerializer.Deserialize<CreateProductCommand>(
                request.Payload,
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? throw new InvalidOperationException("Payload is invalid.");

            // Map files -> CreateProductImageDto (Application DTO)
            if (request.Files is not null && request.Files.Count > 0)
            {
                command.Images = new List<CreateProductImageDto>();

                for (var index = 0; index < request.Files.Count; index++)
                {
                    var file = request.Files[index];
                    if (file is null || file.Length <= 0) continue;

                    var stream = file.OpenReadStream();

                    command.Images.Add(new CreateProductImageDto
                    {
                        Content = stream,
                        FileName = file.FileName,
                        ContentType = file.ContentType,
                        SortOrder = index
                    });
                }
            }

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

        // Logic sanitize / build path đã được chuyển xuống Application layer (CreateProductHandler)
    }
}
