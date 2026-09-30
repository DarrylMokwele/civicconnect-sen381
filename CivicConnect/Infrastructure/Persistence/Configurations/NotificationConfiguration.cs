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
    public class NotificationConfiguration: IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");

            builder.HasKey(n => n.NotificationId);

            builder.Property(n => n.NotificationType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(n => n.Message)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(n => n.CreatedAt)
                .IsRequired();

            builder.Property(n => n.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(n => n.ServiceRequest)
                .WithMany(r => r.Notifications)
                .HasForeignKey(n => n.ServiceRequestId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
