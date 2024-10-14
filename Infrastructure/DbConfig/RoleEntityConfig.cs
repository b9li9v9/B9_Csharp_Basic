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
    internal class RoleEntityConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder
                .ToTable("Roles", SchemaNames.Security)
                .HasIndex(e => e.NormalizedName)
                .HasDatabaseName("IX_Roles_e.NormalizedName");

            
            builder.HasQueryFilter(e => e.IsDeleted == false);
        }
    }
}
