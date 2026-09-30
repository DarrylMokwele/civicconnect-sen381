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
    public class AssignmentConfiguration: IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.ToTable("Assignments");

            builder.HasKey(a => a.AssignmentId);

            builder.Property(a => a.AssignedAt)
                .IsRequired();

            builder.HasOne(a => a.ServiceRequest)
                .WithMany(r => r.Assignments)
                .HasForeignKey(a => a.ServiceRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Department)
                .WithMany(d => d.Assignments)
                .HasForeignKey(a => a.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.AssignedUser)
                .WithMany(u => u.Assignments)
                .HasForeignKey(a => a.AssignedUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
