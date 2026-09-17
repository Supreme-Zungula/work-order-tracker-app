using System.ComponentModel.DataAnnotations;

namespace WorkOrderApi.Dtos;

public class UpdateWorkOrderStatusDto
{
    [Required]
    public string Status { get; set; } = string.Empty;
}
