using Lamie.Application.Common.Storage;
using Lamie.Domain.Entities;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.Settings.Products.Commands;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _repository;
    private readonly IFileStorage _fileStorage;

    public CreateProductHandler(IProductRepository repository, IFileStorage fileStorage)
    {
        _repository = repository;
        _fileStorage = fileStorage;
    }

    public async Task<Guid> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = new Product(command.Sku, command.Price, command.Stock, command.CategoryId);

        if (command.SalePrice.HasValue)
        {
            product.SetSalePrice(command.SalePrice.Value);
        }

        foreach (var t in command.Translations)
        {
            product.AddTranslation(
                t.LanguageCode,
                t.Name,
                t.Slug,
                t.Description ?? string.Empty);
        }

        foreach (var id in (command.TagIds ?? new()).Distinct()) product.AddTag(id);
        foreach (var id in (command.ColorIds ?? new()).Distinct()) product.AddColor(id);
        foreach (var id in (command.CollectionIds ?? new()).Distinct()) product.AddCollection(id);
        foreach (var id in (command.StyleIds ?? new()).Distinct()) product.AddStyle(id);
        foreach (var id in (command.OccasionIds ?? new()).Distinct()) product.AddOccasion(id);

        if (command.ThumbnailFile is { Length: > 0 })
        {
            var thumbPath = ProductObjectPath.Build(command.Sku, command.ThumbnailFile.FileName, -1);
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

        await UploadImagesAsync(product, command.Sku, command.Images, cancellationToken);

        if (string.IsNullOrWhiteSpace(product.ThumbnailUrl))
        {
            var firstImage = product.Images.OrderBy(x => x.SortOrder).FirstOrDefault();
            if (firstImage is not null)
            {
                product.SetThumbnail(firstImage.ImageUrl);
            }
        }

        await _repository.AddAsync(product, cancellationToken);
        return product.Id;
    }

    private async Task UploadImagesAsync(
        Product product,
        string sku,
        List<Dtos.CreateProductImageDto> images,
        CancellationToken cancellationToken)
    {
        if (images is null || images.Count == 0) return;

        for (var index = 0; index < images.Count; index++)
        {
            var imageDto = images[index];

            if (imageDto.ImageFile is null || imageDto.ImageFile.Length == 0)
            {
                if (!string.IsNullOrWhiteSpace(imageDto.ImageUrl))
                {
                    product.AddImage(imageDto.ImageUrl, imageDto.SortOrder ?? index);
                }
                continue;
            }

            var objectPath = ProductObjectPath.Build(sku, imageDto.ImageFile.FileName, index);
            await using var stream = imageDto.ImageFile.OpenReadStream();

            var url = await _fileStorage.UploadPublicAsync(
                stream,
                objectPath,
                imageDto.ImageFile.ContentType ?? "application/octet-stream",
                cancellationToken);

            product.AddImage(url, imageDto.SortOrder ?? index);
        }
    }
}
