using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class CivicConnectDbContext : DbContext
    {
        public CivicConnectDbContext(
            DbContextOptions<CivicConnectDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        public DbSet<Role> Roles => Set<Role>();

        public DbSet<ServiceRequest> ServiceRequests => Set<ServiceRequest>();

        public DbSet<RequestStatus> RequestStatuses => Set<RequestStatus>();

        public DbSet<StatusHistory> StatusHistories => Set<StatusHistory>();

        public DbSet<Department> Departments => Set<Department>();

        public DbSet<Assignment> Assignments => Set<Assignment>();

        public DbSet<Notification> Notifications => Set<Notification>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(CivicConnectDbContext).Assembly);
        }
    }
}
