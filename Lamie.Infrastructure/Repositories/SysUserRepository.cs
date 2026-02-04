using Lamie.Application.Common.Interfaces;
using Lamie.Domain.Entities;
using Lamie.Infrastructure.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lamie.Infrastructure.Repositories
{
    public class SysUserRepository : ISysUserRepository
    {
        private readonly AppDbContext _context;

        public SysUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SysUser?> GetByIdAsync(int id)
            => await _context.SysUsers.FindAsync(id);

        public async Task<List<SysUser>> GetAllAsync()
            => await _context.SysUsers.AsNoTracking().ToListAsync();

        public async Task AddAsync(SysUser user)
        {
            _context.SysUsers.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SysUser user)
        {
            _context.SysUsers.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(SysUser user)
        {
            _context.SysUsers.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
            => await _context.SysUsers.AnyAsync(x => x.Username == username);
    }
}
