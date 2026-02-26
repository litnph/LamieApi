using Lamie.Application.Products.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lamie.Application.Products.Commands
{
    public class CreateProductCommand
    {
        public string Sku { get; set; } = default!;
        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        public int Stock { get; set; }

        public List<CreateProductTranslationDto> Translations { get; set; } = new();
        public List<CreateProductImageDto> Images { get; set; } = new();
    }
}
