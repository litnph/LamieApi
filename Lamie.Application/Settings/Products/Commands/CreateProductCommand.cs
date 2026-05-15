using Lamie.Application.Settings.Products.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Lamie.Application.Settings.Products.Commands;

public class CreateProductCommand : IRequest<Guid>
{
    public string Sku { get; set; } = default!;
    public decimal Price { get; set; }
    public decimal? SalePrice { get; set; }
    public int Stock { get; set; }
    public Guid CategoryId { get; set; }
    public IFormFile? ThumbnailFile { get; set; }
    public string? ThumbnailUrl { get; set; }

    public List<Guid> TagIds { get; set; } = new();
    public List<Guid> ColorIds { get; set; } = new();
    public List<Guid> CollectionIds { get; set; } = new();
    public List<Guid> StyleIds { get; set; } = new();
    public List<Guid> OccasionIds { get; set; } = new();

    public List<CreateProductTranslationDto> Translations { get; set; } = new();
    public List<CreateProductImageDto> Images { get; set; } = new();
}
