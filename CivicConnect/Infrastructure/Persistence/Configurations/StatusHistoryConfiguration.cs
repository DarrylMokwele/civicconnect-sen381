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
    public class StatusHistoryConfiguration: IEntityTypeConfiguration<StatusHistory>
    {
        public void Configure(EntityTypeBuilder<StatusHistory> builder)
        {
            builder.ToTable("StatusHistories");

            builder.HasKey(h => h.StatusHistoryId);

            builder.Property(h => h.ChangedAt)
                .IsRequired();

            builder.HasOne(h => h.ServiceRequest)
                .WithMany(r => r.StatusHistory)
                .HasForeignKey(h => h.ServiceRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(h => h.Status)
                .WithMany(s => s.StatusHistories)
                .HasForeignKey(h => h.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(h => h.ChangedByUser)
                .WithMany(u => u.StatusChanges)
                .HasForeignKey(h => h.ChangedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
