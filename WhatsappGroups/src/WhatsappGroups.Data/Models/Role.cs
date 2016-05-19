using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhatsappGroups.Data.Models
{
    public class Role
    {
        public virtual ICollection<RoleClaim> Claims { get; }
        public string ConcurrencyStamp { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UserRole> Users { get; }
    }
}
