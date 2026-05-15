using Lamie.Application.Common.Exceptions;
using Lamie.Application.Common.Storage;
using Lamie.Domain.Entities;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.Settings.Products.Commands;

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
        var product = await _repository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException("Product", command.Id);

        if (!string.IsNullOrWhiteSpace(command.Sku) && command.Sku != product.Sku)
        {
            product.RenameSku(command.Sku);
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
            product.AdjustStock(command.Stock);
        }

        if (command.CategoryId != product.CategoryId)
        {
            product.Recategorize(command.CategoryId);
        }

        if (command.ThumbnailFile is { Length: > 0 })
        {
            var thumbPath = ProductObjectPath.Build(product.Sku, command.ThumbnailFile.FileName, -1);
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

        SyncRelations(product, command);

        await UpdateImagesAsync(product, command, cancellationToken);

        if (string.IsNullOrWhiteSpace(product.ThumbnailUrl))
        {
            var firstImage = product.Images.OrderBy(x => x.SortOrder).FirstOrDefault();
            if (firstImage is not null)
            {
                product.SetThumbnail(firstImage.ImageUrl);
            }
        }

        await _repository.UpdateAsync(product, cancellationToken);
    }

    private static void SyncRelations(Product product, UpdateProductCommand command)
    {
        product.ClearTags();
        foreach (var id in (command.TagIds ?? new()).Distinct()) product.AddTag(id);

        product.ClearColors();
        foreach (var id in (command.ColorIds ?? new()).Distinct()) product.AddColor(id);

        product.ClearCollections();
        foreach (var id in (command.CollectionIds ?? new()).Distinct()) product.AddCollection(id);

        product.ClearStyles();
        foreach (var id in (command.StyleIds ?? new()).Distinct()) product.AddStyle(id);

        product.ClearOccasions();
        foreach (var id in (command.OccasionIds ?? new()).Distinct()) product.AddOccasion(id);
    }

    private async Task UpdateImagesAsync(Product product, UpdateProductCommand command, CancellationToken cancellationToken)
    {
        if (command.Images is null || command.Images.Count == 0) return;

        var existingIds = product.Images.Select(i => i.Id).ToHashSet();
        var incomingIds = command.Images.Where(x => x.Id.HasValue).Select(x => x.Id!.Value).ToHashSet();

        var toDeactivate = existingIds.Except(incomingIds);
        foreach (var id in toDeactivate)
        {
            product.DeactivateImage(id);
        }

        for (var index = 0; index < command.Images.Count; index++)
        {
            var imageDto = command.Images[index];
            var sortOrder = imageDto.SortOrder ?? index;

            if (imageDto.Id.HasValue && existingIds.Contains(imageDto.Id.Value))
            {
                if (imageDto.ImageFile is { Length: > 0 })
                {
                    var objectPath = ProductObjectPath.Build(product.Sku, imageDto.ImageFile.FileName, sortOrder);
                    await using var stream = imageDto.ImageFile.OpenReadStream();
                    var url = await _fileStorage.UploadPublicAsync(
                        stream,
                        objectPath,
                        imageDto.ImageFile.ContentType ?? "application/octet-stream",
                        cancellationToken);

                    product.UpdateImage(imageDto.Id.Value, url, sortOrder);
                }
                else if (!string.IsNullOrWhiteSpace(imageDto.ImageUrl))
                {
                    product.UpdateImage(imageDto.Id.Value, imageDto.ImageUrl!, sortOrder);
                }
                continue;
            }

            if (imageDto.ImageFile is { Length: > 0 })
            {
                var objectPath = ProductObjectPath.Build(product.Sku, imageDto.ImageFile.FileName, sortOrder);
                await using var stream = imageDto.ImageFile.OpenReadStream();
                var url = await _fileStorage.UploadPublicAsync(
                    stream,
                    objectPath,
                    imageDto.ImageFile.ContentType ?? "application/octet-stream",
                    cancellationToken);

                product.AddImage(url, sortOrder);
            }
            else if (!string.IsNullOrWhiteSpace(imageDto.ImageUrl))
            {
                product.AddImage(imageDto.ImageUrl!, sortOrder);
            }
        }
    }
}
