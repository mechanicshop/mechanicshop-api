using System.ComponentModel.DataAnnotations;

namespace MechanicShop.Api.DTOs.Requests.RepairTasks;

public class CreateRepairTaskPartRequest
{
    [Required(ErrorMessage = "Part name is required.")]
    public string Name { get; init; } = string.Empty;

    [Required(ErrorMessage = "Cost is required.")]
    [Range(1, 10000, ErrorMessage = "Cost must be between 1 and 10,000.")]
    public decimal Cost { get; init; }

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(1, 10, ErrorMessage = "Quantity must be between 1 and 10.")]
    public int Quantity { get; init; }
}