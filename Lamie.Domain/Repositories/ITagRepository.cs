using Lamie.Domain.Entities;

namespace Lamie.Domain.Repositories;

public interface ITagRepository
{
    Task<Tag?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Tag>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Tag tag, CancellationToken cancellationToken = default);
    Task UpdateAsync(Tag tag, CancellationToken cancellationToken = default);
    Task DeleteAsync(Tag tag, CancellationToken cancellationToken = default);
}
