using Lamie.Domain.Entities;

namespace Lamie.Domain.Repositories;

public interface IColorRepository
{
    Task<Color?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Color>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Color color, CancellationToken cancellationToken = default);
    Task UpdateAsync(Color color, CancellationToken cancellationToken = default);
    Task DeleteAsync(Color color, CancellationToken cancellationToken = default);
}
