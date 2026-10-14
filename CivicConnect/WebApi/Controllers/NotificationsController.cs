using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Contracts;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly CivicConnectDbContext _db;
    public NotificationsController(CivicConnectDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<NotificationDto>>> Get([FromQuery] Guid? userId, CancellationToken cancellationToken)
    {
        var query = _db.Notifications.AsNoTracking().AsQueryable();
        if (userId.HasValue) query = query.Where(n => n.UserId == userId.Value);
        var notifications = await query.OrderByDescending(n => n.CreatedAt)
            .Take(50)
            .Select(n => new NotificationDto(n.NotificationId, n.ServiceRequestId, n.NotificationType, n.Message, n.CreatedAt, n.Status))
            .ToListAsync(cancellationToken);
        return Ok(notifications);
    }
}
