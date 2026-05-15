using Lamie.Domain.Entities;
using Lamie.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lamie.Infrastructure.Persistence.Repositories;

public class ColorRepository : IColorRepository
{
    private readonly AppDbContext _context;

    public ColorRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Color?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Colors
            .Include(c => c.Translations)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public Task<List<Color>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _context.Colors
            .Include(c => c.Translations)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Color color, CancellationToken cancellationToken = default)
    {
        await _context.Colors.AddAsync(color, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Color color, CancellationToken cancellationToken = default)
    {
        _context.Colors.Update(color);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Color color, CancellationToken cancellationToken = default)
    {
        _context.Colors.Remove(color);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
