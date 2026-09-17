namespace WorkOrderApi.Models;

public class WorkOrder
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public WorkOrderStatus Status { get; set; } = WorkOrderStatus.Pending;

    public string AssignedTo { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? CompletedAt { get; set; }

    public ICollection<Activity> Activities { get; set; } = new List<Activity>();
}
