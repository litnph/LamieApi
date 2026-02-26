using Lamie.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lamie.Domain.Entities
{
    public class Product
    {
        private readonly List<ProductTranslation> _translations = new();
        private readonly List<ProductImage> _images = new();

        public int Id { get; private set; }
        public string Sku { get; private set; }
        public decimal Price { get; private set; }
        public decimal? SalePrice { get; private set; }
        public int Stock { get; private set; }
        public bool IsActive { get; private set; }

        public IReadOnlyCollection<ProductTranslation> Translations => _translations;
        public IReadOnlyCollection<ProductImage> Images => _images;

        private Product() { } // EF

        public Product(string sku, decimal price, int stock)
        {
            if (string.IsNullOrWhiteSpace(sku))
                throw new DomainException("SKU is required");

            if (price <= 0)
                throw new DomainException("Price must be greater than 0");

            Sku = sku;
            Price = price;
            Stock = stock;
            IsActive = true;
        }

        public void AddTranslation(string languageCode, string name, string description)
        {
            if (_translations.Any(x => x.LanguageCode == languageCode))
                throw new DomainException("Translation already exists");

            _translations.Add(new ProductTranslation(languageCode, name, description));
        }

        public void AddImage(string imageUrl, int sortOrder)
        {
            _images.Add(new ProductImage(imageUrl, sortOrder));
        }

        public void SetSalePrice(decimal salePrice)
        {
            if (salePrice <= 0)
                throw new DomainException("Sale price must be greater than 0");

            if (salePrice >= Price)
                throw new DomainException("Sale price must be less than original price");

            SalePrice = salePrice;
        }

        public void RemoveSalePrice()
        {
            SalePrice = null;
        }

        public void ChangePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new DomainException("Price must be greater than 0");

            if (SalePrice.HasValue && SalePrice.Value >= newPrice)
                throw new DomainException("New price must be greater than sale price");

            Price = newPrice;
        }
    }

}
