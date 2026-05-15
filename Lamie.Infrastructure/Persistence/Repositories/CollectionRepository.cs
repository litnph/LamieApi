using Lamie.Domain.Entities;
using Lamie.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lamie.Infrastructure.Persistence.Repositories;

public class CollectionRepository : ICollectionRepository
{
    private readonly AppDbContext _context;

    public CollectionRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Collection?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Collections
            .Include(c => c.Translations)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public Task<List<Collection>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _context.Collections
            .Include(c => c.Translations)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Collection collection, CancellationToken cancellationToken = default)
    {
        await _context.Collections.AddAsync(collection, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Collection collection, CancellationToken cancellationToken = default)
    {
        _context.Collections.Update(collection);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Collection collection, CancellationToken cancellationToken = default)
    {
        _context.Collections.Remove(collection);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
