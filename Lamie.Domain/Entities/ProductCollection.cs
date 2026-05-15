namespace Lamie.Domain.Entities;

public class ProductCollection : Entity
{
    public Guid ProductId { get; private set; }
    public Guid CollectionId { get; private set; }

    private ProductCollection() { }

    internal ProductCollection(Guid collectionId)
    {
        CollectionId = collectionId;
    }
}
