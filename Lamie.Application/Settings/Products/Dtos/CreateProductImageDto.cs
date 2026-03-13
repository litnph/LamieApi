using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lamie.Application.Settings.Products.Dtos
{
    public class CreateProductImageDto
    {
        public int? Id { get; set; }
        public string? ImageUrl { get; set; } = default!;
        public int? SortOrder { get; set; }

        // Dữ liệu file ở dạng trung lập, không phụ thuộc IFormFile
        public Stream? Content { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
    }
}
