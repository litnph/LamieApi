namespace Lamie.Domain.Entities;

public class ProductTag : Entity
{
    public Guid ProductId { get; private set; }
    public Guid TagId { get; private set; }

    private ProductTag() { }

    internal ProductTag(Guid tagId)
    {
        TagId = tagId;
    }
}
