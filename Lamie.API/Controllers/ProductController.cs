using Lamie.Application.Products.Commands;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Lamie.Application.Products.Queries;
using Lamie.API.Services;
using Lamie.Application.Products.Dtos;

namespace Lamie.API.Controllers
{
    [ApiController]
    [Route("api/admin/products")]
    public class AdminProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IObjectStorageService _objectStorage;

        public AdminProductController(IMediator mediator, IObjectStorageService objectStorage)
        {
            _mediator = mediator;
            _objectStorage = objectStorage;
        }

        /// <summary>
        /// Admin: Tạo sản phẩm mới
        /// </summary>
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateProductCommand command, CancellationToken cancellationToken)
        {
            await UploadImagesIfNeededAsync(command, cancellationToken);

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

        private async Task UploadImagesIfNeededAsync(CreateProductCommand command, CancellationToken cancellationToken)
        {
            if (command.Images is null || command.Images.Count == 0)
            {
                return;
            }

            for (var index = 0; index < command.Images.Count; index++)
            {
                var imageDto = command.Images[index];

                if (imageDto.File is null || imageDto.File.Length <= 0)
                {
                    continue;
                }

                var objectPath = BuildProductObjectPath(command.Sku, imageDto.File.FileName, index);
                var url = await _objectStorage.UploadPublicAsync(imageDto.File, objectPath, cancellationToken);

                imageDto.ImageUrl = url;

                if (imageDto.SortOrder == 0)
                {
                    imageDto.SortOrder = index;
                }
            }
        }

        private static string BuildProductObjectPath(string sku, string? originalFileName, int index)
        {
            var safeSku = string.IsNullOrWhiteSpace(sku) ? "unknown-sku" : SanitizeSegment(sku);
            var fileName = string.IsNullOrWhiteSpace(originalFileName) ? $"image-{index}" : originalFileName;

            var safeName = SanitizeSegment(System.IO.Path.GetFileName(fileName));
            var ext = System.IO.Path.GetExtension(safeName);
            var nameNoExt = System.IO.Path.GetFileNameWithoutExtension(safeName);
            var stamp = Guid.NewGuid().ToString("N");

            var finalName = string.IsNullOrWhiteSpace(ext)
                ? $"{nameNoExt}-{stamp}"
                : $"{nameNoExt}-{stamp}{ext}";

            return $"products/{safeSku}/{finalName}";
        }

        private static string SanitizeSegment(string value)
        {
            var cleaned = value.Trim().Replace(' ', '-').Replace('\\', '-').Replace('/', '-');
            return string.IsNullOrWhiteSpace(cleaned) ? "x" : cleaned;
        }
    }
}
