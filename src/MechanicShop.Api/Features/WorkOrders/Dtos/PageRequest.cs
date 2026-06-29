using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.WorkOrders.Dtos;

public record PageRequest(
    [FromQuery] int Page = 1,
    [FromQuery] int PageSize = 10);
