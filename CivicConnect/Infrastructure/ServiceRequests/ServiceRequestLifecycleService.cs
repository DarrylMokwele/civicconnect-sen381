using Application.Abstractions;
using Application.Notifications;
using Domain.Entities;
using Domain.Events;
using Domain.Exceptions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ServiceRequests
{
    public class ServiceRequestLifecycleService
    : IServiceRequestLifecycleService
    {
        private readonly CivicConnectDbContext _dbContext;
        private readonly IStatusTransitionPolicy _transitionPolicy;
        private readonly ServiceRequestStatusChangedHandler _notificationHandler;

        public ServiceRequestLifecycleService(
            CivicConnectDbContext dbContext,
            IStatusTransitionPolicy transitionPolicy,
            ServiceRequestStatusChangedHandler notificationHandler)
        {
            _dbContext = dbContext;
            _transitionPolicy = transitionPolicy;
            _notificationHandler = notificationHandler;
        }

        public async Task ChangeStatusAsync(
            Guid serviceRequestId,
            Guid newStatusId,
            Guid changedByUserId,
            CancellationToken cancellationToken = default)
        {
            var request = await _dbContext.ServiceRequests
                .Include(r => r.Status)
                .SingleOrDefaultAsync(
                    r => r.ServiceRequestId == serviceRequestId,
                    cancellationToken);

            if (request is null)
            {
                throw new InvalidOperationException(
                    "Service request was not found.");
            }

            var newStatus = await _dbContext.RequestStatuses
                .SingleOrDefaultAsync(
                    s => s.StatusId == newStatusId,
                    cancellationToken);

            if (newStatus is null)
            {
                throw new InvalidOperationException(
                    "Requested status was not found.");
            }

            if (!_transitionPolicy.CanTransition(
                    request.Status.StatusName,
                    newStatus.StatusName))
            {
                throw new InvalidStatusTransitionException(
                    request.Status.StatusName,
                    newStatus.StatusName);
            }

            request.StatusId = newStatus.StatusId;
            request.Status = newStatus;
            request.UpdatedAt = DateTime.UtcNow;

            var history = new StatusHistory
            {
                StatusHistoryId = Guid.NewGuid(),
                ServiceRequestId = request.ServiceRequestId,
                StatusId = newStatus.StatusId,
                ChangedByUserId = changedByUserId,
                ChangedAt = DateTime.UtcNow
            };

            _dbContext.StatusHistories.Add(history);

            await _dbContext.SaveChangesAsync(
                cancellationToken);

            var domainEvent =
                new ServiceRequestStatusChangedEvent(
                    request.ServiceRequestId,
                    request.RequesterId,
                    newStatus.StatusId,
                    DateTime.UtcNow);

            await _notificationHandler.HandleAsync(
                domainEvent,
                cancellationToken);
        }
    }
}
