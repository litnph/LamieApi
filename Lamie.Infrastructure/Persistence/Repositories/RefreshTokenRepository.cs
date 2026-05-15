using Lamie.Domain.Entities.Auth;
using Lamie.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lamie.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _context;

    public RefreshTokenRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken = default)
    {
        return _context.RefreshTokens.FirstOrDefaultAsync(t => t.TokenHash == tokenHash, cancellationToken);
    }

    public async Task AddAsync(RefreshToken token, CancellationToken cancellationToken = default)
    {
        await _context.RefreshTokens.AddAsync(token, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(RefreshToken token, CancellationToken cancellationToken = default)
    {
        _context.RefreshTokens.Update(token);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RevokeAllForUserAsync(Guid userId, DateTime utcNow, CancellationToken cancellationToken = default)
    {
        var tokens = await _context.RefreshTokens
            .Where(t => t.UserId == userId && t.RevokedAt == null && t.ExpiresAt > utcNow)
            .ToListAsync(cancellationToken);

        foreach (var token in tokens)
        {
            token.Revoke(utcNow);
        }

        if (tokens.Count > 0)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
