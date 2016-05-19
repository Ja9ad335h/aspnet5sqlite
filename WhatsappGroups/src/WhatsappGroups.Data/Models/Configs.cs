using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhatsappGroups.Data.Models
{
    public static class Configs
    {
        public static Dictionary<string, string> SqlConnections = new Dictionary<string, string>()
        {
            {"WhatsappGroupsData", "Data Source=C:\\DataBase\\WhatsappGroupsData.db" },
            {"WhatsappGroupsAdmin", "Data Source=C:\\DataBase\\WhatsappGroupsAdmin.db" }
        };
        public static readonly string Username = "harryprod";
        public static readonly string Key = "186622073666856";
        public static readonly int Mode = 0;
    }
}
