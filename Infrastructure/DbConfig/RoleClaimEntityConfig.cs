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
    internal class RoleClaimEntityConfig : IEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder
                .ToTable("RoleClaims", SchemaNames.Security)
                .HasIndex(e => e.RoleId)
                .HasDatabaseName("IX_RoleClaims_RoleId");

            builder.HasQueryFilter(e => e.IsDeleted == false);
            //builder
            //    .HasIndex(e => e.LastName)
            //    .HasDatabaseName("IX_Employees_LastName");
        }
    }
}
