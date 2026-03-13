using Lamie.Application.Common.Exceptions;
using Lamie.Application.Common.Storage;
using Lamie.Domain.Repositories;
using MediatR;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lamie.Application.Settings.Products.Commands
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IProductRepository _repository;
        private readonly IFileStorage _fileStorage;

        public UpdateProductHandler(IProductRepository repository, IFileStorage fileStorage)
        {
            _repository = repository;
            _fileStorage = fileStorage;
        }

        public async Task Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(command.Id);
            if (product is null)
            {
                throw new NotFoundException("Product", command.Id);
            }

            // Cập nhật thông tin cơ bản
            if (!string.IsNullOrWhiteSpace(command.Sku) && command.Sku != product.Sku)
            {
                // Đơn giản: gán trực tiếp (giữ logic validate ở phía API/validator)
                typeof(Domain.Entities.Product)
                    .GetProperty(nameof(product.Sku))!
                    .SetValue(product, command.Sku);
            }

            if (command.Price != product.Price)
            {
                product.ChangePrice(command.Price);
            }

            if (command.SalePrice.HasValue)
            {
                product.SetSalePrice(command.SalePrice.Value);
            }
            else
            {
                product.RemoveSalePrice();
            }

            if (command.Stock != product.Stock)
            {
                typeof(Domain.Entities.Product)
                    .GetProperty(nameof(product.Stock))!
                    .SetValue(product, command.Stock);
            }

            if (command.CategoryId != product.CategoryId)
            {
                typeof(Domain.Entities.Product)
                    .GetProperty(nameof(product.CategoryId))!
                    .SetValue(product, command.CategoryId);
            }

            if (!string.IsNullOrWhiteSpace(command.ThumbnailUrl))
            {
                product.SetThumbnail(command.ThumbnailUrl);
            }

            // TODO: có thể mở rộng cập nhật Translations / Tags / Colors ... sau

            await UpdateImagesAsync(product, command, cancellationToken);

            // Nếu sau khi xử lý ảnh mà thumbnail chưa có, fallback về ảnh đầu tiên
            if (string.IsNullOrWhiteSpace(product.ThumbnailUrl))
            {
                var firstImage = product.Images.OrderBy(x => x.SortOrder).FirstOrDefault();
                if (firstImage is not null)
                {
                    product.SetThumbnail(firstImage.ImageUrl);
                }
            }

            await _repository.UpdateAsync(product);
        }

        private async Task UpdateImagesAsync(Domain.Entities.Product product, UpdateProductCommand command, CancellationToken cancellationToken)
        {
            if (command.Images is null || command.Images.Count == 0)
            {
                return;
            }

            var existingIds = product.Images.Select(i => i.Id).ToHashSet();
            var incomingIds = command.Images.Where(x => x.Id.HasValue).Select(x => x.Id!.Value).ToHashSet();

            // Deactivate những ảnh không còn trong danh sách gửi lên
            var toDeactivate = existingIds.Except(incomingIds);
            foreach (var id in toDeactivate)
            {
                product.DeactivateImage(id);
            }

            foreach (var (imageDto, index) in command.Images.Select((img, idx) => (img, idx)))
            {
                var sortOrder = imageDto.SortOrder ?? index;

                // Ảnh cũ (có Id)
                if (imageDto.Id.HasValue && existingIds.Contains(imageDto.Id.Value))
                {
                    // Nếu có content mới => upload Supabase, cập nhật url
                    if (imageDto.Content is { Length: > 0 })
                    {
                        var objectPath = BuildProductObjectPath(product.Sku, imageDto.FileName, sortOrder);
                        await using var stream = new MemoryStream(imageDto.Content);

                        var url = await _fileStorage.UploadPublicAsync(
                            stream,
                            objectPath,
                            imageDto.ContentType ?? "application/octet-stream",
                            cancellationToken);

                        product.UpdateImage(imageDto.Id.Value, url, sortOrder);
                    }
                    else if (!string.IsNullOrWhiteSpace(imageDto.ImageUrl))
                    {
                        // Chỉ update thông tin (url/sort)
                        product.UpdateImage(imageDto.Id.Value, imageDto.ImageUrl!, sortOrder);
                    }

                    continue;
                }

                // Ảnh mới
                if (imageDto.Content is { Length: > 0 })
                {
                    var objectPath = BuildProductObjectPath(product.Sku, imageDto.FileName, sortOrder);
                    await using var stream = new MemoryStream(imageDto.Content);

                    var url = await _fileStorage.UploadPublicAsync(
                        stream,
                        objectPath,
                        imageDto.ContentType ?? "application/octet-stream",
                        cancellationToken);

                    product.AddImage(url, sortOrder);
                }
                else if (!string.IsNullOrWhiteSpace(imageDto.ImageUrl))
                {
                    product.AddImage(imageDto.ImageUrl!, sortOrder);
                }
            }
        }

        private static string BuildProductObjectPath(string sku, string? originalFileName, int index)
        {
            var safeSku = string.IsNullOrWhiteSpace(sku) ? "unknown-sku" : SanitizeSegment(sku);
            var fileName = string.IsNullOrWhiteSpace(originalFileName) ? $"image-{index}" : originalFileName;

            var safeName = SanitizeSegment(Path.GetFileName(fileName));
            var ext = Path.GetExtension(safeName);
            var nameNoExt = Path.GetFileNameWithoutExtension(safeName);
            var stamp = System.Guid.NewGuid().ToString("N");

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

