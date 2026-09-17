using System.ComponentModel.DataAnnotations;

namespace WorkOrderApi.Dtos;

public class CreateWorkOrderDto
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    [Required, MaxLength(100)]
    public string AssignedTo { get; set; } = string.Empty;

    public DateTime? DueDate { get; set; }
}
