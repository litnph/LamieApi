namespace Lamie.Domain.Entities;

public class ProductStyle : Entity
{
    public Guid ProductId { get; private set; }
    public Guid StyleId { get; private set; }

    private ProductStyle() { }

    internal ProductStyle(Guid styleId)
    {
        StyleId = styleId;
    }
}
