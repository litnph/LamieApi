namespace Lamie.Application.Settings.Products.Dtos;

public class CreateProductTranslationDto
{
    public string LanguageCode { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string? Description { get; set; }
}
