namespace Lamie.Domain.Entities;

public class ProductColor : Entity
{
    public Guid ProductId { get; private set; }
    public Guid ColorId { get; private set; }

    private ProductColor() { }

    internal ProductColor(Guid colorId)
    {
        ColorId = colorId;
    }
}
