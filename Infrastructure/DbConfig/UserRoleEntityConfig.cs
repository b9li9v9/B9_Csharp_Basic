using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DbConfig
{
    internal class UserRoleEntityConfig : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder
                .ToTable("UserRoles", SchemaNames.Security)
                .HasIndex(e => e.UserId)
                .HasDatabaseName("IX_UserRoles_UserId");

            builder.HasQueryFilter(e => e.IsDeleted == false);
        }
    }
}
