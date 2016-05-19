using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WhatsappGroups.Data.Models
{
    public class UserInstance
    {
        [Key]
        public string UniqueKey { get; set; }
        public string Agent { get; set; }
        public string HostAddress { get; set; }
        public string HostName { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
