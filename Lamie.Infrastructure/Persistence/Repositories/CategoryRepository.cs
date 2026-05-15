using Lamie.Domain.Entities;
using Lamie.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lamie.Infrastructure.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Categories
            .Include(c => c.Translations)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _context.Categories
            .Include(c => c.Translations)
            .OrderBy(c => c.SortOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        await _context.Categories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Category category, CancellationToken cancellationToken = default)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Category category, CancellationToken cancellationToken = default)
    {
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
