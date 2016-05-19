using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using WhatsappGroups.Data.Models;
using Microsoft.Extensions.Configuration;

namespace WhatsappGroups.Data.Contexts
{
    public class WhatsappGroupsDataContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity<OperatorCircle>().HasKey(oc => new { oc.OperatorId, oc.CircleId });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(Configs.SqlConnections["WhatsappGroupsData"]);
        }
    }
}
