using Microsoft.EntityFrameworkCore;
using WorkOrderApi.Models;

namespace WorkOrderApi.Data;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.Migrate();

        if (context.WorkOrders.Any())
        {
            return;
        }

        var now = DateTime.UtcNow;

        var workOrders = new List<WorkOrder>
        {
            new()
            {
                Title = "Replace HVAC filters in Building A",
                Description = "Quarterly maintenance: replace all filters on floors 1-3.",
                Status = WorkOrderStatus.Pending,
                AssignedTo = "Alice Nguyen",
                CreatedAt = now.AddDays(-5),
                DueDate = now.AddDays(2)
            },
            new()
            {
                Title = "Fix leaking faucet in Room 204",
                Description = "Reported by facilities. Check both supply lines and the cartridge.",
                Status = WorkOrderStatus.InProgress,
                AssignedTo = "Marcus Silva",
                CreatedAt = now.AddDays(-3),
                DueDate = now.AddDays(1)
            },
            new()
            {
                Title = "Quarterly electrical panel inspection",
                Description = "Inspect panels B and C, log thermal readings.",
                Status = WorkOrderStatus.Completed,
                AssignedTo = "Priya Patel",
                CreatedAt = now.AddDays(-14),
                DueDate = now.AddDays(-2),
                CompletedAt = now.AddDays(-3)
            }
        };

        context.WorkOrders.AddRange(workOrders);
        context.SaveChanges();

        var activities = new List<Activity>
        {
            new()
            {
                WorkOrderId = workOrders[0].Id,
                Action = "Created",
                Details = "Work order created.",
                PerformedBy = "System",
                Timestamp = now.AddDays(-5)
            },
            new()
            {
                WorkOrderId = workOrders[1].Id,
                Action = "Created",
                Details = "Work order created.",
                PerformedBy = "System",
                Timestamp = now.AddDays(-3)
            },
            new()
            {
                WorkOrderId = workOrders[1].Id,
                Action = "Status Changed",
                Details = "Pending -> InProgress",
                PerformedBy = "Marcus Silva",
                Timestamp = now.AddDays(-1)
            },
            new()
            {
                WorkOrderId = workOrders[2].Id,
                Action = "Created",
                Details = "Work order created.",
                PerformedBy = "System",
                Timestamp = now.AddDays(-14)
            },
            new()
            {
                WorkOrderId = workOrders[2].Id,
                Action = "Status Changed",
                Details = "Pending -> InProgress",
                PerformedBy = "Priya Patel",
                Timestamp = now.AddDays(-7)
            },
            new()
            {
                WorkOrderId = workOrders[2].Id,
                Action = "Status Changed",
                Details = "InProgress -> Completed",
                PerformedBy = "Priya Patel",
                Timestamp = now.AddDays(-3)
            }
        };

        context.Activities.AddRange(activities);
        context.SaveChanges();
    }
}
