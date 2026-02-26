using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lamie.Application.Products.Dtos
{
    public class CreateProductImageDto
    {
        public string ImageUrl { get; set; } = default!;
        public int SortOrder { get; set; }
    }
}
