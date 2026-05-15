using Microsoft.AspNetCore.Http;

namespace Lamie.Application.Settings.Products.Dtos;

public class CreateProductImageDto
{
    public Guid? Id { get; set; }
    public string? ImageUrl { get; set; }
    public int? SortOrder { get; set; }
    public IFormFile? ImageFile { get; set; }
}
