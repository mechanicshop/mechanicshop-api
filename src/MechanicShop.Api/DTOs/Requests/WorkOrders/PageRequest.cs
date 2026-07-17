using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.DTOs.Requests.WorkOrders;

public record PageRequest(
    [FromQuery] int Page = 1,
    [FromQuery] int PageSize = 10);
