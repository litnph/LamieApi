using Lamie.Domain.Entities;

namespace Lamie.Domain.Repositories;

public interface IChannelRepository
{
    Task<Channel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Channel?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<List<Channel>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsCodeAsync(string code, Guid? exceptId, CancellationToken cancellationToken = default);
    Task AddAsync(Channel channel, CancellationToken cancellationToken = default);
    Task UpdateAsync(Channel channel, CancellationToken cancellationToken = default);
    Task DeleteAsync(Channel channel, CancellationToken cancellationToken = default);
}
