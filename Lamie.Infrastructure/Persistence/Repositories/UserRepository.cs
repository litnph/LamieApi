using Lamie.Domain.Entities.Auth;
using Lamie.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lamie.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalized = email.Trim().ToLowerInvariant();
        return _context.Users.FirstOrDefaultAsync(u => u.Email == normalized, cancellationToken);
    }

    public Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        var trimmed = userName.Trim();
        return _context.Users.FirstOrDefaultAsync(u => u.UserName == trimmed, cancellationToken);
    }

    public Task<User?> GetByEmailOrUserNameAsync(string login, CancellationToken cancellationToken = default)
    {
        var trimmed = login.Trim();
        var lowered = trimmed.ToLowerInvariant();
        return _context.Users.FirstOrDefaultAsync(
            u => u.Email == lowered || u.UserName == trimmed,
            cancellationToken);
    }

    public Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _context.Users
            .AsNoTracking()
            .OrderBy(u => u.Email)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> ExistsEmailAsync(string email, Guid? exceptId, CancellationToken cancellationToken = default)
    {
        var normalized = email.Trim().ToLowerInvariant();
        return _context.Users.AsNoTracking()
            .AnyAsync(u => u.Email == normalized && (exceptId == null || u.Id != exceptId), cancellationToken);
    }

    public Task<bool> ExistsUserNameAsync(string userName, Guid? exceptId, CancellationToken cancellationToken = default)
    {
        var trimmed = userName.Trim();
        return _context.Users.AsNoTracking()
            .AnyAsync(u => u.UserName == trimmed && (exceptId == null || u.Id != exceptId), cancellationToken);
    }

    public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return _context.Users.AsNoTracking().AnyAsync(cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
