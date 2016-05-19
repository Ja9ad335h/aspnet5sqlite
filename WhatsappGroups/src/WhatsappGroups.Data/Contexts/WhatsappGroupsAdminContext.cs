using Microsoft.Data.Entity;
using WhatsappGroups.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhatsappGroups.Data.Contexts
{
    public class WhatsappGroupsAdminContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<UserInstance> ClientInstances { get; set; }
        public DbSet<RSAPrivateKey> RSAPrivateKeys { get; set; }
        public DbSet<Audience> Audiences { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserRole>().HasKey(ur => new { ur.RoleId, ur.UserId });
            builder.Entity<User>().Property(u => u.CreateDate).HasDefaultValueSql("getutcdate()");
            builder.Entity<Audience>().Property(u => u.CreateDate).HasDefaultValueSql("getutcdate()");
            builder.Entity<RSAPrivateKey>().Property(u => u.CreateDate).HasDefaultValueSql("getutcdate()");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(Configs.SqlConnections["WhatsappGroupsAdmin"]);
        }
    }
}
