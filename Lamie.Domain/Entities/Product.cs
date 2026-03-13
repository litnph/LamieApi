using Lamie.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lamie.Domain.Entities
{
    public class Product : Entity
    {
        private readonly List<ProductTranslation> _translations = new();
        private readonly List<ProductImage> _images = new();
        private readonly List<ProductCollection> _collections = new();
        private readonly List<ProductColor> _colors = new();
        private readonly List<ProductTag> _tags = new();
        private readonly List<ProductStyle> _styles = new();
        private readonly List<ProductOccasion> _occasions = new();

        public int Id { get; private set; }
        public string Sku { get; private set; } = default!;
        public decimal Price { get; private set; }
        public decimal? SalePrice { get; private set; }
        public int Stock { get; private set; }
        public int CategoryId { get; private set; }
        public bool IsActive { get; private set; }
        public string? ThumbnailUrl { get; private set; }

        public IReadOnlyCollection<ProductTranslation> Translations => _translations;
        public IReadOnlyCollection<ProductImage> Images => _images;

        public IReadOnlyCollection<ProductCollection> Collections => _collections;
        public IReadOnlyCollection<ProductColor> Colors => _colors;
        public IReadOnlyCollection<ProductTag> Tags => _tags;
        public IReadOnlyCollection<ProductStyle> Styles => _styles;
        public IReadOnlyCollection<ProductOccasion> Occasions => _occasions;

        private Product() { } // EF

        public Product(string sku, decimal price, int stock, int categoryId)
        {
            if (string.IsNullOrWhiteSpace(sku))
                throw new DomainException("SKU is required");

            if (price <= 0)
                throw new DomainException("Price must be greater than 0");

            if (categoryId <= 0)
                throw new DomainException("CategoryId is required");

            Sku = sku;
            Price = price;
            Stock = stock;
            CategoryId = categoryId;
            IsActive = true;
        }

        public void AddTranslation(string languageCode, string name, string slug, string description)
        {
            if (_translations.Any(x => x.LanguageCode == languageCode))
                throw new DomainException("Translation already exists");

            _translations.Add(new ProductTranslation(languageCode, name, slug, description));
        }

        public void AddImage(string imageUrl, int sortOrder)
        {
            _images.Add(new ProductImage(imageUrl, sortOrder));
        }

        public void UpdateImage(int imageId, string imageUrl, int sortOrder)
        {
            var image = _images.FirstOrDefault(x => x.Id == imageId);
            if (image is null)
                throw new DomainException($"Image with id {imageId} not found.");

            image.Update(imageUrl, sortOrder);
        }

        public void DeactivateImage(int imageId)
        {
            var image = _images.FirstOrDefault(x => x.Id == imageId);
            if (image is null)
                return;

            image.Deactivate();
        }

        public void AddCollection(int collectionId)
        {
            if (collectionId <= 0) throw new DomainException("CollectionId is required");
            if (_collections.Any(x => x.CollectionId == collectionId)) return;
            _collections.Add(new ProductCollection(collectionId));
        }

        public void AddColor(int colorId)
        {
            if (colorId <= 0) throw new DomainException("ColorId is required");
            if (_colors.Any(x => x.ColorId == colorId)) return;
            _colors.Add(new ProductColor(colorId));
        }

        public void AddTag(int tagId)
        {
            if (tagId <= 0) throw new DomainException("TagId is required");
            if (_tags.Any(x => x.TagId == tagId)) return;
            _tags.Add(new ProductTag(tagId));
        }

        public void AddStyle(int styleId)
        {
            if (styleId <= 0) throw new DomainException("StyleId is required");
            if (_styles.Any(x => x.StyleId == styleId)) return;
            _styles.Add(new ProductStyle(styleId));
        }

        public void AddOccasion(int occasionId)
        {
            if (occasionId <= 0) throw new DomainException("OccasionId is required");
            if (_occasions.Any(x => x.OccasionId == occasionId)) return;
            _occasions.Add(new ProductOccasion(occasionId));
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

        public void SetThumbnail(string? url)
        {
            ThumbnailUrl = url;
        }
    }

}
