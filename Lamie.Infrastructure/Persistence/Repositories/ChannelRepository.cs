using Lamie.Domain.Entities;
using Lamie.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lamie.Infrastructure.Persistence.Repositories;

public class ChannelRepository : IChannelRepository
{
    private readonly AppDbContext _context;

    public ChannelRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Channel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Channels.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public Task<Channel?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(code)) return Task.FromResult<Channel?>(null);
        var normalized = code.Trim().ToUpperInvariant();
        return _context.Channels.FirstOrDefaultAsync(c => c.Code == normalized, cancellationToken);
    }

    public Task<List<Channel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _context.Channels
            .AsNoTracking()
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsCodeAsync(string code, Guid? exceptId, CancellationToken cancellationToken = default)
    {
        var normalized = code.Trim().ToUpperInvariant();
        return await _context.Channels
            .AsNoTracking()
            .AnyAsync(c => c.Code == normalized && (exceptId == null || c.Id != exceptId), cancellationToken);
    }

    public async Task AddAsync(Channel channel, CancellationToken cancellationToken = default)
    {
        await _context.Channels.AddAsync(channel, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Channel channel, CancellationToken cancellationToken = default)
    {
        _context.Channels.Update(channel);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Channel channel, CancellationToken cancellationToken = default)
    {
        _context.Channels.Remove(channel);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
