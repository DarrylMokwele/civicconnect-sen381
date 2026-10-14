using Application.Abstractions;
using Application.ServiceRequests;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Contracts;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceRequestsController : ControllerBase
{
    private readonly IServiceRequestLifecycleService _lifecycleService;
    private readonly CivicConnectDbContext _db;

    public ServiceRequestsController(
        IServiceRequestLifecycleService lifecycleService,
        CivicConnectDbContext db)
    {
        _lifecycleService = lifecycleService;
        _db = db;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ServiceRequestDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ServiceRequestDto>>> GetRequests(
        [FromQuery] Guid? requesterId,
        [FromQuery] Guid? statusId,
        [FromQuery] string? search,
        CancellationToken cancellationToken)
    {
        var query = _db.ServiceRequests
            .AsNoTracking()
            .Include(r => r.Status)
            .AsQueryable();

        if (requesterId.HasValue)
            query = query.Where(r => r.RequesterId == requesterId.Value);

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

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ServiceRequestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServiceRequestDto>> GetRequest(Guid id, CancellationToken cancellationToken)
    {
        var request = await _db.ServiceRequests
            .AsNoTracking()
            .Include(r => r.Status)
            .Where(r => r.ServiceRequestId == id)
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
            .SingleOrDefaultAsync(cancellationToken);

        return request is null ? NotFound(new { error = "Service request was not found." }) : Ok(request);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ServiceRequestDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ServiceRequestDto>> CreateRequest(
        [FromBody] CreateServiceRequestRequest input,
        CancellationToken cancellationToken)
    {
        if (input.RequesterId == Guid.Empty)
            return BadRequest(new { error = "RequesterId is required." });

        if (string.IsNullOrWhiteSpace(input.Title))
            return BadRequest(new { error = "Title is required." });

        if (string.IsNullOrWhiteSpace(input.Description))
            return BadRequest(new { error = "Description is required." });

        var requesterExists = await _db.Users.AnyAsync(u => u.UserId == input.RequesterId, cancellationToken);
        if (!requesterExists)
            return BadRequest(new { error = "Requester was not found." });

        var submitted = await _db.RequestStatuses
            .SingleOrDefaultAsync(s => s.StatusName == "Submitted" && s.IsActive, cancellationToken);

        if (submitted is null)
            return Problem("The Submitted status is not configured.", statusCode: StatusCodes.Status500InternalServerError);

        if (input.DepartmentId.HasValue &&
            !await _db.Departments.AnyAsync(d => d.DepartmentId == input.DepartmentId.Value, cancellationToken))
            return BadRequest(new { error = "Department was not found." });

        var now = DateTime.UtcNow;
        var entity = new ServiceRequest
        {
            ServiceRequestId = Guid.NewGuid(),
            RequesterId = input.RequesterId,
            StatusId = submitted.StatusId,
            DepartmentId = input.DepartmentId,
            Title = input.Title.Trim(),
            Description = input.Description.Trim(),
            CreatedAt = now,
            UpdatedAt = now
        };

        _db.ServiceRequests.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);

        var dto = new ServiceRequestDto(
            entity.ServiceRequestId,
            entity.RequesterId,
            submitted.StatusId,
            submitted.StatusName,
            entity.DepartmentId,
            entity.Title,
            entity.Description,
            entity.CreatedAt,
            entity.UpdatedAt);

        return CreatedAtAction(nameof(GetRequest), new { id = entity.ServiceRequestId }, dto);
    }

    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeStatus(
        Guid id,
        [FromBody] ChangeServiceRequestStatusRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            await _lifecycleService.ChangeStatusAsync(
                id,
                request.NewStatusId,
                request.ChangedByUserId,
                cancellationToken);

            return NoContent();
        }
        catch (InvalidStatusTransitionException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}
