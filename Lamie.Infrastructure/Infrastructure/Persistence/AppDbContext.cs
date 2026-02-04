using Lamie.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lamie.Infrastructure.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<SysUser> SysUsers => Set<SysUser>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
