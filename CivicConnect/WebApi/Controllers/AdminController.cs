using System.Security.Claims;
using Application.Abstractions;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Contracts;

namespace WebApi.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly CivicConnectDbContext _db;
    private readonly IServiceRequestLifecycleService _lifecycleService;

    public AdminController(
        IConfiguration configuration,
        CivicConnectDbContext db,
        IServiceRequestLifecycleService lifecycleService)
    {
        _configuration = configuration;
        _db = db;
        _lifecycleService = lifecycleService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] AdminLoginRequest input,
        CancellationToken cancellationToken)
    {
        var username = input.Username?.Trim() ?? string.Empty;
        var adminUsername = _configuration["CivicConnect:Admin:Username"] ?? "admin";
        var adminPassword = _configuration["CivicConnect:Admin:Password"] ?? "Admin@123";
        var staffUsername = _configuration["CivicConnect:Staff:Username"] ?? "staff";
        var staffPassword = _configuration["CivicConnect:Staff:Password"] ?? "Staff@123";

        var isAdmin = string.Equals(username, adminUsername, StringComparison.Ordinal) &&
                      string.Equals(input.Password, adminPassword, StringComparison.Ordinal);
        var isStaff = string.Equals(username, staffUsername, StringComparison.Ordinal) &&
                      string.Equals(input.Password, staffPassword, StringComparison.Ordinal);

        if (!isAdmin && !isStaff)
            return Unauthorized(new { error = "Invalid username or password." });

        var configuredUserIdKey = isAdmin ? "CivicConnect:Admin:UserId" : "CivicConnect:Staff:UserId";
        var fallbackUserId = isAdmin ? DbInitializer.AdminUserId : DbInitializer.StaffUserId;
        var userId = Guid.TryParse(_configuration[configuredUserIdKey], out var configuredId)
            ? configuredId
            : fallbackUserId;

        var user = await _db.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.UserId == userId, cancellationToken);

        if (user is null)
            return Problem("The requested account has not been seeded. Enable development seed data and restart the API.", statusCode: 500);

        var role = isAdmin ? "Admin" : "Staff";
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}".Trim()),
            new(ClaimTypes.Role, role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = false,
                AllowRefresh = true
            });

        return Ok(new
        {
            userId = user.UserId,
            name = $"{user.FirstName} {user.LastName}".Trim(),
            role
        });
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return NoContent();
    }

    [Authorize(Roles = "Admin,Staff")]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            authenticated = User.Identity?.IsAuthenticated == true,
            userId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            name = User.Identity?.Name,
            role = User.FindFirstValue(ClaimTypes.Role) ?? "Staff"
        });
    }

    [Authorize(Roles = "Admin,Staff")]
    [HttpGet("overview")]
    public async Task<IActionResult> Overview(CancellationToken cancellationToken)
    {
        var requests = _db.ServiceRequests.AsNoTracking();

        var result = new
        {
            total = await requests.CountAsync(cancellationToken),
            submitted = await requests.CountAsync(r => r.Status.StatusName == "Submitted", cancellationToken),
            assigned = await requests.CountAsync(r => r.Status.StatusName == "Assigned", cancellationToken),
            inProgress = await requests.CountAsync(r => r.Status.StatusName == "In Progress", cancellationToken),
            resolved = await requests.CountAsync(r => r.Status.StatusName == "Resolved", cancellationToken),
            closed = await requests.CountAsync(r => r.Status.StatusName == "Closed", cancellationToken),
            pendingNotifications = await _db.Notifications.AsNoTracking()
                .CountAsync(n => n.Status == "Pending", cancellationToken)
        };

        return Ok(result);
    }

    [Authorize(Roles = "Admin,Staff")]
    [HttpGet("requests")]
    public async Task<ActionResult<IEnumerable<ServiceRequestDto>>> Requests(
        [FromQuery] Guid? statusId,
        [FromQuery] string? search,
        CancellationToken cancellationToken)
    {
        var query = _db.ServiceRequests
            .AsNoTracking()
            .Include(r => r.Status)
            .AsQueryable();

        if (statusId.HasValue)
            query = query.Where(r => r.StatusId == statusId.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(r => r.Title.Contains(term) || r.Description.Contains(term));
        }

        var results = await query
            .OrderByDescending(r => r.UpdatedAt)
            .Select(r => new ServiceRequestDto(
                r.ServiceRequestId,
                r.RequesterId,
                r.StatusId,
                r.Status.StatusName,
                r.DepartmentId,
                r.Title,
                r.Description,
                r.CreatedAt,
                r.UpdatedAt))
            .ToListAsync(cancellationToken);

        return Ok(results);
    }

    [Authorize(Roles = "Admin,Staff")]
    [HttpPatch("requests/{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(
        Guid id,
        [FromBody] AdminStatusUpdateRequest input,
        CancellationToken cancellationToken)
    {
        if (input.NewStatusId == Guid.Empty)
            return BadRequest(new { error = "NewStatusId is required." });

        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var adminUserId))
            return Unauthorized(new { error = "Administrator identity is unavailable." });

        try
        {
            await _lifecycleService.ChangeStatusAsync(
                id,
                input.NewStatusId,
                adminUserId,
                cancellationToken);

            return NoContent();
        }
        catch (Domain.Exceptions.InvalidStatusTransitionException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [Authorize(Roles = "Admin,Staff")]
    [HttpGet("notifications")]
    public async Task<IActionResult> Notifications(CancellationToken cancellationToken)
    {
        var notifications = await _db.Notifications
            .AsNoTracking()
            .OrderByDescending(n => n.CreatedAt)
            .Take(50)
            .Select(n => new NotificationDto(
                n.NotificationId,
                n.ServiceRequestId,
                n.NotificationType,
                n.Message,
                n.CreatedAt,
                n.Status))
            .ToListAsync(cancellationToken);

        return Ok(notifications);
    }
    [Authorize(Roles = "Admin")]
    [HttpGet("staff")]
    public async Task<IActionResult> Staff(CancellationToken cancellationToken)
    {
        var staff = await _db.Users.AsNoTracking()
            .Include(u => u.Role)
            .Where(u => u.Role.RoleName == "Staff")
            .OrderBy(u => u.FirstName).ThenBy(u => u.LastName)
            .Select(u => new
            {
                userId = u.UserId,
                name = (u.FirstName + " " + u.LastName).Trim(),
                email = u.Email,
                role = u.Role.RoleName
            })
            .ToListAsync(cancellationToken);
        return Ok(staff);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("requests/{id:guid}/assignment")]
    public async Task<IActionResult> Assignment(Guid id, CancellationToken cancellationToken)
    {
        var assignment = await _db.Assignments.AsNoTracking()
            .Include(a => a.Department)
            .Include(a => a.AssignedUser)
            .Where(a => a.ServiceRequestId == id && a.UnassignedAt == null)
            .OrderByDescending(a => a.AssignedAt)
            .Select(a => new
            {
                assignmentId = a.AssignmentId,
                departmentId = a.DepartmentId,
                departmentName = a.Department.DepartmentName,
                assignedUserId = a.AssignedUserId,
                assignedUserName = a.AssignedUser == null ? null : (a.AssignedUser.FirstName + " " + a.AssignedUser.LastName).Trim(),
                assignedAt = a.AssignedAt
            })
            .FirstOrDefaultAsync(cancellationToken);
        return Ok(assignment);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("requests/{id:guid}/assignment")]
    public async Task<IActionResult> SetAssignment(
        Guid id,
        [FromBody] AdminAssignmentRequest input,
        CancellationToken cancellationToken)
    {
        var request = await _db.ServiceRequests.SingleOrDefaultAsync(r => r.ServiceRequestId == id, cancellationToken);
        if (request is null) return NotFound(new { error = "Service request was not found." });

        if (!await _db.Departments.AnyAsync(d => d.DepartmentId == input.DepartmentId, cancellationToken))
            return BadRequest(new { error = "Department was not found." });

        if (input.AssignedUserId.HasValue &&
            !await _db.Users.Include(u => u.Role).AnyAsync(
                u => u.UserId == input.AssignedUserId.Value && u.Role.RoleName == "Staff",
                cancellationToken))
            return BadRequest(new { error = "Assigned staff member was not found or is not a staff account." });

        var active = await _db.Assignments
            .Where(a => a.ServiceRequestId == id && a.UnassignedAt == null)
            .OrderByDescending(a => a.AssignedAt)
            .FirstOrDefaultAsync(cancellationToken);

        var now = DateTime.UtcNow;
        if (active is not null)
        {
            active.UnassignedAt = now;
            _db.Assignments.Update(active);
        }

        var newAssignment = new Domain.Entities.Assignment
        {
            AssignmentId = Guid.NewGuid(),
            ServiceRequestId = id,
            DepartmentId = input.DepartmentId,
            AssignedUserId = input.AssignedUserId,
            AssignedAt = now
        };

        _db.Assignments.Add(newAssignment);
        request.DepartmentId = input.DepartmentId;
        request.UpdatedAt = now;
        await _db.SaveChangesAsync(cancellationToken);

        return Ok(new { message = "Assignment updated.", assignmentId = newAssignment.AssignmentId });
    }

    [Authorize(Roles = "Admin,Staff")]
    [HttpGet("my-assignments")]
    public async Task<IActionResult> MyAssignments(CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            return Unauthorized(new { error = "Staff identity is unavailable." });

        var assignments = await _db.Assignments
            .AsNoTracking()
            .Include(a => a.ServiceRequest)
                .ThenInclude(r => r.Status)
            .Include(a => a.Department)
            .Where(a => a.AssignedUserId == userId && a.UnassignedAt == null)
            .OrderByDescending(a => a.AssignedAt)
            .Select(a => new
            {
                assignmentId = a.AssignmentId,
                serviceRequestId = a.ServiceRequestId,
                title = a.ServiceRequest.Title,
                description = a.ServiceRequest.Description,
                statusId = a.ServiceRequest.StatusId,
                statusName = a.ServiceRequest.Status.StatusName,
                departmentId = a.DepartmentId,
                departmentName = a.Department.DepartmentName,
                assignedAt = a.AssignedAt
            })
            .ToListAsync(cancellationToken);

        return Ok(assignments);
    }

}
