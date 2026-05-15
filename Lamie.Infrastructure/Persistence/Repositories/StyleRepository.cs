using Lamie.Domain.Entities;
using Lamie.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lamie.Infrastructure.Persistence.Repositories;

public class StyleRepository : IStyleRepository
{
    private readonly AppDbContext _context;

    public StyleRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Style?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Styles
            .Include(s => s.Translations)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public Task<List<Style>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _context.Styles
            .Include(s => s.Translations)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Style style, CancellationToken cancellationToken = default)
    {
        await _context.Styles.AddAsync(style, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Style style, CancellationToken cancellationToken = default)
    {
        _context.Styles.Update(style);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Style style, CancellationToken cancellationToken = default)
    {
        _context.Styles.Remove(style);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
