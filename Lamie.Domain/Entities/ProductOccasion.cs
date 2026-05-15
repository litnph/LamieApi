namespace Lamie.Domain.Entities;

public class ProductOccasion : Entity
{
    public Guid ProductId { get; private set; }
    public Guid OccasionId { get; private set; }

    private ProductOccasion() { }

    internal ProductOccasion(Guid occasionId)
    {
        OccasionId = occasionId;
    }
}
