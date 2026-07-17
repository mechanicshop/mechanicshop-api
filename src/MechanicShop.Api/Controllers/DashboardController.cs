using Asp.Versioning;

using MechanicShop.Application.Features.Dashboard.Queries.GetWorkOrderStats;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Controllers;

[Route("api/v{version:apiVersion}/dashboard")]
[ApiVersion("1.0")]
[Authorize("ManagerOnly")]
public class DashboardController : ApiController
{
    private readonly ISender _sender;

    public DashboardController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("stats")]
    public async Task<ActionResult> GetWorkOrderStats(CancellationToken ct)
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        var result = await _sender.Send(new GetWorkOrderStatsQuery(date), ct);

        return result.Match<ActionResult>(Ok, Problem);
    }
}
