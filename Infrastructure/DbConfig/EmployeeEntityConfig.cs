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
    internal class EmployeeEntityConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder
                .ToTable("Employees", SchemaNames.Service)
                .HasIndex(e => e.Name)
                .HasDatabaseName("IX_Employees_Name");
            
            builder.HasQueryFilter(e => e.IsDeleted == false); 
        }
    }
}
