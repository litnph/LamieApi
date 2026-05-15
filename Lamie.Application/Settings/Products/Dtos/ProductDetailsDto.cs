namespace Lamie.Application.Settings.Products.Dtos;

public sealed record ProductTranslationDto
{
    public Guid Id { get; init; }
    public string LanguageCode { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public string Description { get; init; } = default!;
}

public sealed record ProductImageDto
{
    public Guid Id { get; init; }
    public string ImageUrl { get; init; } = default!;
    public bool IsActive { get; init; }
    public int SortOrder { get; init; }
}

public sealed record ProductDetailsDto
{
    public Guid Id { get; init; }
    public string Sku { get; init; } = default!;
    public decimal Price { get; init; }
    public decimal? SalePrice { get; init; }
    public int Stock { get; init; }
    public Guid CategoryId { get; init; }
    public bool IsActive { get; init; }
    public string? ThumbnailUrl { get; init; }

    public IReadOnlyList<ProductTranslationDto> Translations { get; init; } = [];
    public IReadOnlyList<ProductImageDto> Images { get; init; } = [];

    public IReadOnlyList<Guid> TagIds { get; init; } = [];
    public IReadOnlyList<Guid> ColorIds { get; init; } = [];
    public IReadOnlyList<Guid> CollectionIds { get; init; } = [];
    public IReadOnlyList<Guid> StyleIds { get; init; } = [];
    public IReadOnlyList<Guid> OccasionIds { get; init; } = [];
}
