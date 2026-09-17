using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkOrderApi.Data;
using WorkOrderApi.Dtos;

namespace WorkOrderApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivitiesController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public ActivitiesController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<object>> GetActivities(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? workOrderId = null,
        CancellationToken cancellationToken = default)
    {
        if (page < 1 || pageSize < 1 || pageSize > 100)
        {
            return BadRequest(new
            {
                error = "page must be >= 1 and pageSize must be between 1 and 100."
            });
        }

        var query = _db.Activities.AsNoTracking();

        if (workOrderId.HasValue)
        {
            query = query.Where(a => a.WorkOrderId == workOrderId.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(a => a.Timestamp)
            .ThenByDescending(a => a.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new ActivityDto
            {
                Id = a.Id,
                WorkOrderId = a.WorkOrderId,
                Action = a.Action,
                Details = a.Details,
                PerformedBy = a.PerformedBy,
                Timestamp = a.Timestamp
            })
            .ToListAsync(cancellationToken);

        return Ok(new
        {
            items,
            page,
            pageSize,
            totalCount
        });
    }
}
