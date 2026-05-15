using Lamie.Domain.Entities.Auth;

namespace Lamie.Domain.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken = default);
    Task AddAsync(RefreshToken token, CancellationToken cancellationToken = default);
    Task UpdateAsync(RefreshToken token, CancellationToken cancellationToken = default);
    Task RevokeAllForUserAsync(Guid userId, DateTime utcNow, CancellationToken cancellationToken = default);
}
