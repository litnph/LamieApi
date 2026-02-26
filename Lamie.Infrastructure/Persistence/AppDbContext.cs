using Lamie.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lamie.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<SysUser> SysUsers => Set<SysUser>();
        public DbSet<Product> Products => Set<Product>();

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
