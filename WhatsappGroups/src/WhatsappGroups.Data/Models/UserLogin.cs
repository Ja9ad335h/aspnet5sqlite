using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WhatsappGroups.Data.Models
{
    public class UserLogin
    {
        public string LoginProvider { get; set; }
        public string ProviderDisplayName { get; set; }
        [Key]
        public string ProviderKey { get; set; }
        public int UserId { get; set; }
    }
}
