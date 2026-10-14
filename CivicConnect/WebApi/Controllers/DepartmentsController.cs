using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly CivicConnectDbContext _db;
    public DepartmentsController(CivicConnectDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var departments = await _db.Departments.AsNoTracking()
            .OrderBy(d => d.DepartmentName)
            .Select(d => new { d.DepartmentId, d.DepartmentName })
            .ToListAsync(cancellationToken);
        return Ok(departments);
    }
}
