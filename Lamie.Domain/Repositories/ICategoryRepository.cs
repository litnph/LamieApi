using Lamie.Domain.Entities;

namespace Lamie.Domain.Repositories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Category category, CancellationToken cancellationToken = default);
    Task UpdateAsync(Category category, CancellationToken cancellationToken = default);
    Task DeleteAsync(Category category, CancellationToken cancellationToken = default);
}
