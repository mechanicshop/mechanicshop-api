using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.WorkOrders.Dtos;

public record WorkOrderFilterRequest(
    [FromQuery] string? SearchTerm,
    [FromQuery] string? SortColumn,
    [FromQuery] string? SortDirection,
    [FromQuery] int? State,
    [FromQuery] Guid? VehicleId,
    [FromQuery] Guid? LaborId,
    [FromQuery] DateTime? StartDateFrom,
    [FromQuery] DateTime? StartDateTo,
    [FromQuery] DateTime? EndDateFrom,
    [FromQuery] DateTime? EndDateTo,
    [FromQuery] int? Spot);
