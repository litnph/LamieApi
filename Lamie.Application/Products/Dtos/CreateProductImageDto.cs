using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Lamie.Application.Products.Dtos
{
    public class CreateProductImageDto
    {
        public int? Id { get; set; }
        public string? ImageUrl { get; set; } = default!;
        public int? SortOrder { get; set; }
        public IFormFile File { get; set; } = default!;
    }
}
