using Lamie.Domain.Entities;

namespace Lamie.Domain.Repositories;

public interface IStyleRepository
{
    Task<Style?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Style>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Style style, CancellationToken cancellationToken = default);
    Task UpdateAsync(Style style, CancellationToken cancellationToken = default);
    Task DeleteAsync(Style style, CancellationToken cancellationToken = default);
}
