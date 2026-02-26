using Lamie.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lamie.Domain.Entities
{
    public class ProductImage
    {
        public string ImageUrl { get; private set; }
        public int SortOrder { get; private set; }

        internal ProductImage(string imageUrl, int sortOrder)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new DomainException("Image URL is required");

            ImageUrl = imageUrl;
            SortOrder = sortOrder;
        }
    }

}
