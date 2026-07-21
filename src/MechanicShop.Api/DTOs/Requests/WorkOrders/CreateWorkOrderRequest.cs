using System.ComponentModel.DataAnnotations;

using MechanicShop.Domain.Workorders.Enums;

namespace MechanicShop.Api.DTOs.Requests.WorkOrders;

public class CreateWorkOrderRequest
{
    [Required(ErrorMessage = "Spot is required.")]
    [Range(0, 3, ErrorMessage = "Invalid range [0, 1, 2 or 3]")]
    public Spot Spot { get; init; }

    [Required(ErrorMessage = "Vehicle is required.")]
    public Guid VehicleId { get; init; }

    [MinLength(1, ErrorMessage = "At least one repair task must be selected.")]
    public List<Guid> RepairTaskIds { get; init; } = [];

    [Required(ErrorMessage = "Labor is required.")]
    public Guid LaborId { get; init; }

    [Required(ErrorMessage = "StartAt is required.")]
    public DateTimeOffset StartAtUtc { get; init; }
}
