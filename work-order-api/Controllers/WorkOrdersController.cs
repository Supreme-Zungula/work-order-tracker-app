using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkOrderApi.Data;
using WorkOrderApi.Dtos;
using WorkOrderApi.Models;

namespace WorkOrderApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkOrdersController : ControllerBase
{
    private const string SystemActor = "System";
    private const string CreatedAction = "Created";
    private const string StatusChangedAction = "Status Changed";

    private readonly ApplicationDbContext _db;

    public WorkOrdersController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<object>> GetWorkOrders(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? status = null,
        CancellationToken cancellationToken = default)
    {
        if (page < 1 || pageSize < 1 || pageSize > 100)
        {
            return BadRequest(new
            {
                error = "page must be >= 1 and pageSize must be between 1 and 100."
            });
        }

        if (status is not null && !TryParseStatusStrict(status, out var statusFilter))
        {
            return BadRequest(new
            {
                error = $"Invalid status '{status}'. Allowed values: {string.Join(", ", AllowedStatusNames)}."
            });
        }

        var query = _db.WorkOrders.AsNoTracking();

        if (status is not null && TryParseStatusStrict(status, out var statusFilter2))
        {
            query = query.Where(w => w.Status == statusFilter2!);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(w => w.CreatedAt)
            .ThenByDescending(w => w.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(w => new WorkOrderDto
            {
                Id = w.Id,
                Title = w.Title,
                Description = w.Description,
                Status = w.Status.ToString(),
                AssignedTo = w.AssignedTo,
                CreatedAt = w.CreatedAt,
                DueDate = w.DueDate,
                CompletedAt = w.CompletedAt,
                Activities = new List<ActivityDto>()
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

    [HttpGet("{id:int}")]
    public async Task<ActionResult<WorkOrderDto>> GetWorkOrder(int id, CancellationToken cancellationToken)
    {
        var workOrder = await _db.WorkOrders
            .AsNoTracking()
            .Include(w => w.Activities)
            .Where(w => w.Id == id)
            .Select(w => new WorkOrderDto
            {
                Id = w.Id,
                Title = w.Title,
                Description = w.Description,
                Status = w.Status.ToString(),
                AssignedTo = w.AssignedTo,
                CreatedAt = w.CreatedAt,
                DueDate = w.DueDate,
                CompletedAt = w.CompletedAt,
                Activities = w.Activities
                    .OrderByDescending(a => a.Timestamp)
                    .ThenByDescending(a => a.Id)
                    .Select(a => new ActivityDto
                    {
                        Id = a.Id,
                        WorkOrderId = a.WorkOrderId,
                        Action = a.Action,
                        Details = a.Details,
                        PerformedBy = a.PerformedBy,
                        Timestamp = a.Timestamp
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (workOrder is null)
        {
            return NotFound(new { error = $"Work order {id} was not found." });
        }

        return Ok(workOrder);
    }

    [HttpPost]
    public async Task<ActionResult<WorkOrderDto>> CreateWorkOrder([FromBody] CreateWorkOrderDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var now = DateTime.UtcNow;

        var workOrder = new WorkOrder
        {
            Title = dto.Title.Trim(),
            Description = dto.Description?.Trim(),
            Status = WorkOrderStatus.Pending,
            AssignedTo = dto.AssignedTo.Trim(),
            CreatedAt = now,
            DueDate = dto.DueDate,
            CompletedAt = null
        };

        workOrder.Activities.Add(new Activity
        {
            Action = CreatedAction,
            Details = "Work order created.",
            PerformedBy = SystemActor,
            Timestamp = now
        });

        _db.WorkOrders.Add(workOrder);
        await _db.SaveChangesAsync(cancellationToken);

        var result = new WorkOrderDto
        {
            Id = workOrder.Id,
            Title = workOrder.Title,
            Description = workOrder.Description,
            Status = workOrder.Status.ToString(),
            AssignedTo = workOrder.AssignedTo,
            CreatedAt = workOrder.CreatedAt,
            DueDate = workOrder.DueDate,
            CompletedAt = workOrder.CompletedAt,
            Activities = workOrder.Activities
                .OrderByDescending(a => a.Timestamp)
                .ThenByDescending(a => a.Id)
                .Select(a => new ActivityDto
                {
                    Id = a.Id,
                    WorkOrderId = a.WorkOrderId,
                    Action = a.Action,
                    Details = a.Details,
                    PerformedBy = a.PerformedBy,
                    Timestamp = a.Timestamp
                })
                .ToList()
        };

        return CreatedAtAction(nameof(GetWorkOrder), new { id = workOrder.Id }, result);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateWorkOrderStatus(int id, [FromBody] UpdateWorkOrderStatusDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        if (!TryParseStatusStrict(dto.Status, out var newStatus))
        {
            return BadRequest(new
            {
                error = $"Invalid status '{dto.Status}'. Allowed values: {string.Join(", ", AllowedStatusNames)}."
            });
        }

        var workOrder = await _db.WorkOrders
            .Include(w => w.Activities)
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);

        if (workOrder is null)
        {
            return NotFound(new { error = $"Work order {id} was not found." });
        }

        if (workOrder.Status != newStatus)
        {
            var previousStatus = workOrder.Status;
            var now = DateTime.UtcNow;

            workOrder.Status = newStatus;

            if (newStatus == WorkOrderStatus.Completed)
            {
                workOrder.CompletedAt = workOrder.CompletedAt ?? now;
            }
            else
            {
                workOrder.CompletedAt = null;
            }

            workOrder.Activities.Add(new Activity
            {
                Action = StatusChangedAction,
                Details = $"{previousStatus} -> {newStatus}",
                PerformedBy = SystemActor,
                Timestamp = now
            });

            await _db.SaveChangesAsync(cancellationToken);
        }

        var result = new WorkOrderDto
        {
            Id = workOrder.Id,
            Title = workOrder.Title,
            Description = workOrder.Description,
            Status = workOrder.Status.ToString(),
            AssignedTo = workOrder.AssignedTo,
            CreatedAt = workOrder.CreatedAt,
            DueDate = workOrder.DueDate,
            CompletedAt = workOrder.CompletedAt,
            Activities = workOrder.Activities
                .OrderByDescending(a => a.Timestamp)
                .ThenByDescending(a => a.Id)
                .Select(a => new ActivityDto
                {
                    Id = a.Id,
                    WorkOrderId = a.WorkOrderId,
                    Action = a.Action,
                    Details = a.Details,
                    PerformedBy = a.PerformedBy,
                    Timestamp = a.Timestamp
                })
                .ToList()
        };

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteWorkOrder(int id, CancellationToken cancellationToken)
    {
        var workOrder = await _db.WorkOrders
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);

        if (workOrder is null)
        {
            return NotFound(new { error = $"Work order {id} was not found." });
        }

        _db.WorkOrders.Remove(workOrder);
        await _db.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    private bool TryParseStatusStrict(string value, out WorkOrderStatus status)
    {
        status = default;

        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var trimmed = value.Trim();

        if (trimmed.Any(char.IsDigit))
        {
            return false;
        }

        return Enum.TryParse(trimmed, ignoreCase: true, out status)
            && Enum.IsDefined(typeof(WorkOrderStatus), status);
    }

    private static readonly string[] AllowedStatusNames =
        Enum.GetNames<WorkOrderStatus>();
}
