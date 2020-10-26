using AdministrationStation.Server.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace AdministrationStation.Server.Data
{
    public class ServerContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }    

        public ServerContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(k => new {k.UserId, k.RoleId});
        }
    }
}