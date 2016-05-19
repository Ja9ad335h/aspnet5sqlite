using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhatsappGroups.Data.Models
{
    public class RoleClaim
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public int Id { get; set; }
        public int RoleId { get; set; }
    }
}
