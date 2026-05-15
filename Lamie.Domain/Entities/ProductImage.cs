using Lamie.Domain.Exceptions;

namespace Lamie.Domain.Entities;

public class ProductImage : Entity
{
    public Guid ProductId { get; private set; }
    public string ImageUrl { get; private set; } = default!;
    public bool IsActive { get; private set; }
    public int SortOrder { get; private set; }

    private ProductImage() { }

    internal ProductImage(string imageUrl, int sortOrder)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new DomainException("Image URL is required");

        ImageUrl = imageUrl;
        SortOrder = sortOrder;
        IsActive = true;
    }

    public void Deactivate() => IsActive = false;

    internal void Update(string imageUrl, int sortOrder)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new DomainException("Image URL is required");

        ImageUrl = imageUrl;
        SortOrder = sortOrder;
    }
}
