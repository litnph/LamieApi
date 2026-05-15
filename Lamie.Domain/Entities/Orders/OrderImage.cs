using Lamie.Domain.Exceptions;

namespace Lamie.Domain.Entities.Orders;

public class OrderImage : Entity
{
    public Guid OrderId { get; private set; }
    public string ImageUrl { get; private set; } = default!;
    public int SortOrder { get; private set; }
    public string? Description { get; private set; }

    private OrderImage() { }

    internal OrderImage(string imageUrl, int sortOrder, string? description)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new DomainException("Image URL is required");

        ImageUrl = imageUrl;
        SortOrder = sortOrder;
        Description = description;
    }

    internal void Update(int sortOrder, string? description)
    {
        SortOrder = sortOrder;
        Description = description;
    }
}
