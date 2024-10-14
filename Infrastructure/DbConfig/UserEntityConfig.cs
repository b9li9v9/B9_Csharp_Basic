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
    internal class UserEntityConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .ToTable("Users", SchemaNames.Security)
                .HasIndex(e => e.Email)
                .HasDatabaseName("IX_Users_Email");

            builder.HasQueryFilter(e => e.IsDeleted == false);
        }
    }
}
