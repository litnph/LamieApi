using Lamie.Domain.Entities;
using Lamie.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lamie.Infrastructure.Persistence.Repositories;

public class TagRepository : ITagRepository
{
    private readonly AppDbContext _context;

    public TagRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Tag?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Tags
            .Include(t => t.Translations)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public Task<List<Tag>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _context.Tags
            .Include(t => t.Translations)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Tag tag, CancellationToken cancellationToken = default)
    {
        await _context.Tags.AddAsync(tag, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Tag tag, CancellationToken cancellationToken = default)
    {
        _context.Tags.Update(tag);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Tag tag, CancellationToken cancellationToken = default)
    {
        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
