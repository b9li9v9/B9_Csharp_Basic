using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
//Add-Migration <MigrationName>
// dotnet ef migrations add <MigrationName> --startup-project <path-to-webapi> --project <path-to-model-project>
namespace Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }

        public DbSet<Employee> Employees { get; set; }
    }
}
