using Lamie.Application.Common.Storage;
using Lamie.Domain.Entities;
using Lamie.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lamie.Application.Settings.Products.Commands
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IProductRepository _repository;

        private readonly IFileStorage _fileStorage;

        public CreateProductHandler(IProductRepository repository, IFileStorage fileStorage)
        {
            _repository = repository;
            _fileStorage = fileStorage;
        }

        public async Task<int> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // Tạo Aggregate
            var product = new Product(
                command.Sku,
                command.Price,
                command.Stock,
                command.CategoryId
            );

            // Nếu có SalePrice thì gọi method Domain (nếu bạn có)
            if (command.SalePrice.HasValue)
            {
                product.SetSalePrice(command.SalePrice.Value);
            }

            // Add translations
            foreach (var t in command.Translations)
            {
                product.AddTranslation(
                    t.LanguageCode,
                    t.Name,
                    t.Slug,
                    t.Description ?? string.Empty
                );
            }

            // Relationships (n-n)
            if (command.TagIds is not null)
            {
                foreach (var id in command.TagIds.Distinct())
                {
                    product.AddTag(id);
                }
            }

            if (command.ColorIds is not null)
            {
                foreach (var id in command.ColorIds.Distinct())
                {
                    product.AddColor(id);
                }
            }

            if (command.CollectionIds is not null)
            {
                foreach (var id in command.CollectionIds.Distinct())
                {
                    product.AddCollection(id);
                }
            }

            if (command.StyleIds is not null)
            {
                foreach (var id in command.StyleIds.Distinct())
                {
                    product.AddStyle(id);
                }
            }

            if (command.OccasionIds is not null)
            {
                foreach (var id in command.OccasionIds.Distinct())
                {
                    product.AddOccasion(id);
                }
            }

            // Upload thumbnail nếu có file
            if (command.ThumbnailFile is { Length: > 0 })
            {
                var thumbPath = BuildProductObjectPath(command.Sku, command.ThumbnailFile.FileName, -1);
                await using var thumbStream = command.ThumbnailFile.OpenReadStream();
                var thumbUrl = await _fileStorage.UploadPublicAsync(
                    thumbStream,
                    thumbPath,
                    command.ThumbnailFile.ContentType ?? "application/octet-stream",
                    cancellationToken);
                product.SetThumbnail(thumbUrl);
            }
            else if (!string.IsNullOrWhiteSpace(command.ThumbnailUrl))
            {
                product.SetThumbnail(command.ThumbnailUrl);
            }

            // Add images sau khi upload (nếu có)
            await UploadImagesIfNeededAsync(product, command, cancellationToken);

            // Nếu chưa set thumbnail mà có ảnh, dùng ảnh đầu tiên theo sort
            if (string.IsNullOrWhiteSpace(product.ThumbnailUrl))
            {
                var firstImage = product.Images.OrderBy(x => x.SortOrder).FirstOrDefault();
                if (firstImage is not null)
                {
                    product.SetThumbnail(firstImage.ImageUrl);
                }
            }

            // Lưu aggregate
            await _repository.AddAsync(product);

            return product.Id;
        }

        private async Task UploadImagesIfNeededAsync(Product product, CreateProductCommand command, CancellationToken cancellationToken)
        {
            if (command.Images is null || command.Images.Count == 0)
            {
                return;
            }

            for (var index = 0; index < command.Images.Count; index++)
            {
                var imageDto = command.Images[index];

                if (imageDto.ImageFile is null || imageDto.ImageFile.Length == 0)
                {
                    // Không có file attach, dùng ImageUrl nếu có
                    if (!string.IsNullOrWhiteSpace(imageDto.ImageUrl))
                    {
                        product.AddImage(
                            imageDto.ImageUrl,
                            imageDto.SortOrder ?? index
                        );
                    }
                    continue;
                }

                var objectPath = BuildProductObjectPath(command.Sku, imageDto.ImageFile.FileName, index);

                await using var stream = imageDto.ImageFile.OpenReadStream();

                var url = await _fileStorage.UploadPublicAsync(
                    stream,
                    objectPath,
                    imageDto.ImageFile.ContentType ?? "application/octet-stream",
                    cancellationToken);

                var sortOrder = imageDto.SortOrder ?? index;

                product.AddImage(url, sortOrder);
            }
        }

        private static string BuildProductObjectPath(string sku, string? originalFileName, int index)
        {
            var safeSku = string.IsNullOrWhiteSpace(sku) ? "unknown-sku" : SanitizeSegment(sku);
            var fileName = string.IsNullOrWhiteSpace(originalFileName) ? $"image-{index}" : originalFileName;

            var safeName = SanitizeSegment(Path.GetFileName(fileName));
            var ext = Path.GetExtension(safeName);
            var nameNoExt = Path.GetFileNameWithoutExtension(safeName);
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
