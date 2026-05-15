using Lamie.Application.Common.Exceptions;
using Lamie.Application.Settings.Products.Dtos;
using Lamie.Domain.Entities;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.Settings.Products.Queries;

public sealed class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, List<ProductDetailsDto>>
{
    private readonly IProductRepository _repository;

    public GetAllProductsHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ProductDetailsDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _repository.GetAllAsync(cancellationToken);
        return products.Select(ProductMapper.ToDetailsDto).ToList();
    }
}

public sealed class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDetailsDto>
{
    private readonly IProductRepository _repository;

    public GetProductByIdHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductDetailsDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Product", request.Id);

        return ProductMapper.ToDetailsDto(product);
    }
}

internal static class ProductMapper
{
    public static ProductDetailsDto ToDetailsDto(Product product)
    {
        return new ProductDetailsDto
        {
            Id = product.Id,
            Sku = product.Sku,
            Price = product.Price,
            SalePrice = product.SalePrice,
            Stock = product.Stock,
            CategoryId = product.CategoryId,
            IsActive = product.IsActive,
            ThumbnailUrl = product.ThumbnailUrl,
            Translations = product.Translations.Select(t => new ProductTranslationDto
            {
                Id = t.Id,
                LanguageCode = t.LanguageCode,
                Name = t.Name,
                Slug = t.Slug,
                Description = t.Description,
            }).ToList(),
            Images = product.Images.Select(i => new ProductImageDto
            {
                Id = i.Id,
                ImageUrl = i.ImageUrl,
                IsActive = i.IsActive,
                SortOrder = i.SortOrder,
            }).OrderBy(i => i.SortOrder).ToList(),
            TagIds = product.Tags.Select(x => x.TagId).Distinct().ToList(),
            ColorIds = product.Colors.Select(x => x.ColorId).Distinct().ToList(),
            CollectionIds = product.Collections.Select(x => x.CollectionId).Distinct().ToList(),
            StyleIds = product.Styles.Select(x => x.StyleId).Distinct().ToList(),
            OccasionIds = product.Occasions.Select(x => x.OccasionId).Distinct().ToList(),
        };
    }
}
