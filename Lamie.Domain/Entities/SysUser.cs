using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lamie.Domain.Entities
{
    public class SysUser
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string Password { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public string? CreatedName { get; set; }

        public DateTime UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public string? UpdatedName { get; set; }
    }
}
