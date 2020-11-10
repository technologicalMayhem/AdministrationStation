using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AdministrationStation.Server.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Logging;

namespace AdministrationStation.Server.Data
{
    public class ServerContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        private static readonly ILoggerFactory Factory = LoggerFactory.Create(builder =>
        {
            builder
                .AddConsole((options) => { })
                .AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Information);
        });

        public ServerContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(Factory);
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(k => new {k.UserId, k.RoleId});
        }
    }
}