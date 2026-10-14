using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Contracts;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatusesController : ControllerBase
{
    private readonly CivicConnectDbContext _db;
    public StatusesController(CivicConnectDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StatusDto>>> Get(CancellationToken cancellationToken)
    {
        var statuses = await _db.RequestStatuses.AsNoTracking()
            .Where(s => s.IsActive)
            .OrderBy(s => s.StatusName)
            .Select(s => new StatusDto(s.StatusId, s.StatusName, s.Description, s.IsActive))
            .ToListAsync(cancellationToken);
        return Ok(statuses);
    }
}
