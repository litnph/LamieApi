using Lamie.Domain.Entities;
using Lamie.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lamie.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Products
            .Include(p => p.Translations)
            .Include(p => p.Images)
            .Include(p => p.Tags)
            .Include(p => p.Colors)
            .Include(p => p.Collections)
            .Include(p => p.Styles)
            .Include(p => p.Occasions)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _context.Products
            .Include(p => p.Translations)
            .Include(p => p.Images)
            .Include(p => p.Tags)
            .Include(p => p.Colors)
            .Include(p => p.Collections)
            .Include(p => p.Styles)
            .Include(p => p.Occasions)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Product product, CancellationToken cancellationToken = default)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
