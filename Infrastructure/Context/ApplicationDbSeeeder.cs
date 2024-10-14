using Infrastructure.Constant;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;

namespace Infrastructure.Context
{
    public class ApplicationDbSeeeder
    {
        private readonly ApplicationDbContext _dbContext;

        public ApplicationDbSeeeder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SeedDatabaseAsync()
        {
            await SeedDatabaseTableAsync();

            await SeedRolesAsync();

            await SeedRoleClaimsAsync();

            await SeedUserAsync();

            await SeedUserRolesAsync();
        }

        private async Task SeedDatabaseTableAsync()
        {
            _dbContext.Database.EnsureDeleted();
            await _dbContext.Database.MigrateAsync();
            //if (_dbContext.Database.GetPendingMigrations().Any())
            //{
            //    try
            //    {
            //        _dbContext.Database.EnsureDeleted();
            //        await _dbContext.Database.MigrateAsync();
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //}


        }

        private async Task SeedRolesAsync()
        {
            foreach (var roleName in AppRoles.DefaultRoles)
            {
                // add roles

                _dbContext.Roles.Add(new Role
                {
                    Id = Guid.NewGuid().ToString(),
                    NormalizedName = roleName.ToUpperInvariant(),
                    Name = roleName,
                });
            }
            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedRoleClaimsAsync()
        {
            foreach (var roleName in AppRoles.DefaultRoles)
            {
                var role = await _dbContext.Roles.FirstOrDefaultAsync(e => e.NormalizedName == roleName.ToUpper());
                // add admmin roleclaims
                if (role.NormalizedName == AppRoles.Admin.ToUpper())
                {
                    foreach (var adminPermissions in AppPermissions.AdminPermissions)
                    {
                        var roleClaims = new RoleClaim()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Description = adminPermissions.Description,
                            Group = adminPermissions.Group,
                            RoleId = role.Id.ToString(),
                            ClaimType = AppClaim.Permission,
                            ClaimValue = adminPermissions.Name
                        };
                        _dbContext.RoleClaims.Add(roleClaims);
                    }
                }

                // add basic roleclaims
                if (role.NormalizedName == AppRoles.Basic.ToUpper())
                {
                    foreach (var adminPermissions in AppPermissions.BasicPermissions)
                    {
                        var roleClaims = new RoleClaim()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Description = adminPermissions.Description,
                            Group = adminPermissions.Group,
                            RoleId = role.Id.ToString(),
                            ClaimType = AppClaim.Permission,
                            ClaimValue = adminPermissions.Name
                        };
                        _dbContext.RoleClaims.Add(roleClaims);
                    }
                }

                await _dbContext.SaveChangesAsync();
            }
        }

        private async Task SeedUserAsync()
        {
            var admin = new User()
            {
                Id = Guid.NewGuid().ToString(),
                Email = AppCredentials.AdminEmail,
                Password = AppCredentials.AdminPassword,
                Name = AppCredentials.AdminName
            };

            var basic = new User()
            {
                Id = Guid.NewGuid().ToString(),
                Email = AppCredentials.BasicEmail,
                Password = AppCredentials.BasicPassword,
                Name = AppCredentials.BasicName
            };

            _dbContext.Users.Add(admin);
            _dbContext.Users.Add(basic);
            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedUserRolesAsync()
        {
            var adminUser = _dbContext.Users.FirstOrDefault(e=>e.Name == AppCredentials.AdminName);
            var basicUser = _dbContext.Users.FirstOrDefault(e => e.Name == AppCredentials.BasicName);
            
            var adminRole = _dbContext.Roles.FirstOrDefault(e => e.NormalizedName == AppRoles.Admin.ToUpper());
            var basicRole = _dbContext.Roles.FirstOrDefault(e => e.NormalizedName == AppRoles.Basic.ToUpper());

            var adminUserRole = new UserRole()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = adminUser.Id,
                RoleId = adminRole.Id
            };

            var adminUserBasicRole = new UserRole()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = adminUser.Id,
                RoleId = basicRole.Id
            };

            var basicUserRole = new UserRole()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = basicUser.Id,
                RoleId = basicRole.Id
            };

            

            _dbContext.UserRoles.Add(adminUserRole);
            _dbContext.UserRoles.Add(adminUserBasicRole);
            _dbContext.UserRoles.Add(basicUserRole);
            await _dbContext.SaveChangesAsync();

        }
    }
}
