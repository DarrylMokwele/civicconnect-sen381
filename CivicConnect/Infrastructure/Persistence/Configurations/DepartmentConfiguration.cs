using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configurations
{
    public class DepartmentConfiguration: IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");

            builder.HasKey(d => d.DepartmentId);

            builder.Property(d => d.DepartmentName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(d => d.Description)
                .HasMaxLength(500);
        }
    }
}
