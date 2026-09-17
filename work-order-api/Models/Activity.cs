namespace WorkOrderApi.Models;

public class Activity
{
    public int Id { get; set; }

    public int WorkOrderId { get; set; }

    public WorkOrder? WorkOrder { get; set; }

    public string Action { get; set; } = string.Empty;

    public string? Details { get; set; }

    public string PerformedBy { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; }
}
