namespace Lamie.Application.Settings.Products.Dtos
{
    public class CreateProductImageDto
    {
        public int? Id { get; set; }
        public string? ImageUrl { get; set; } = default!;
        public int? SortOrder { get; set; }

        // Dữ liệu file ở dạng trung lập (base64 -> byte[] khi bind JSON)
        public byte[]? Content { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
    }
}
