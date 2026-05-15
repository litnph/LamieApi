using Lamie.Domain.Entities;

namespace Lamie.Domain.Repositories;

public interface ICollectionRepository
{
    Task<Collection?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Collection>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Collection collection, CancellationToken cancellationToken = default);
    Task UpdateAsync(Collection collection, CancellationToken cancellationToken = default);
    Task DeleteAsync(Collection collection, CancellationToken cancellationToken = default);
}
