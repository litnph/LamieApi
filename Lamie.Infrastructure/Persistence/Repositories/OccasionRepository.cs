using Lamie.Domain.Entities;
using Lamie.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lamie.Infrastructure.Persistence.Repositories;

public class OccasionRepository : IOccasionRepository
{
    private readonly AppDbContext _context;

    public OccasionRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Occasion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Occasions
            .Include(o => o.Translations)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public Task<List<Occasion>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _context.Occasions
            .Include(o => o.Translations)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Occasion occasion, CancellationToken cancellationToken = default)
    {
        await _context.Occasions.AddAsync(occasion, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Occasion occasion, CancellationToken cancellationToken = default)
    {
        _context.Occasions.Update(occasion);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Occasion occasion, CancellationToken cancellationToken = default)
    {
        _context.Occasions.Remove(occasion);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
