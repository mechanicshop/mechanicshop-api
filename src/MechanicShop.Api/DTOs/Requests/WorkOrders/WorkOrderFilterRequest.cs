using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.DTOs.Requests.WorkOrders;

public record WorkOrderFilterRequest(
    [FromQuery] string? SearchTerm,
    [FromQuery] string? SortColumn,
    [FromQuery] string? SortDirection,
    [FromQuery] int? State,
    [FromQuery] Guid? VehicleId,
    [FromQuery] Guid? LaborId,
    [FromQuery] DateTime? StartDate,
    [FromQuery] DateTime? EndDate,
    [FromQuery] int? Spot);
