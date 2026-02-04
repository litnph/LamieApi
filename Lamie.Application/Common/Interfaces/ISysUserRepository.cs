using Lamie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lamie.Application.Common.Interfaces
{
    public interface ISysUserRepository
    {
        Task<SysUser?> GetByIdAsync(int id);
        Task<List<SysUser>> GetAllAsync();
        Task AddAsync(SysUser user);
        Task UpdateAsync(SysUser user);
        Task DeleteAsync(SysUser user);
        Task<bool> ExistsByUsernameAsync(string username);
    }
}
