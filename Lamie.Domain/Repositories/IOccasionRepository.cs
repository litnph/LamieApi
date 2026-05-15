using Lamie.Domain.Entities;

namespace Lamie.Domain.Repositories;

public interface IOccasionRepository
{
    Task<Occasion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Occasion>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Occasion occasion, CancellationToken cancellationToken = default);
    Task UpdateAsync(Occasion occasion, CancellationToken cancellationToken = default);
    Task DeleteAsync(Occasion occasion, CancellationToken cancellationToken = default);
}
