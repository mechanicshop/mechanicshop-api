using Asp.Versioning;

using MechanicShop.Application.Features.Scheduling.Queries.GetDailyScheduleQuery;
using MechanicShop.Domain.Identity;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Controllers;

[Route("api/v{version:apiVersion}/workorders")]
[ApiVersion("1.0")]
[Authorize(Roles = nameof(Role.Manager) + "," + nameof(Role.Labor))]
public class SchedulingController(ISender sender) : ApiController
{
    [HttpGet("schedule/{date}")]
    public async Task<ActionResult> GetDailySchedule(
        DateOnly? date,
        [FromQuery] Guid? laborId,
        [FromHeader(Name = "X-TimeZone")] string? tz,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(tz))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Time Zone Required",
                detail: "Missing time zone in 'X-TimeZone' header.");
        }

        TimeZoneInfo timeZone;

        try
        {
            timeZone = TimeZoneInfo.FindSystemTimeZoneById(tz);
        }
        catch
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid Time Zone",
                detail: $"Invalid or unknown time zone: '{tz}'.");
        }

        var scheduleDate = date ?? DateOnly.FromDateTime(DateTime.UtcNow);

        var result = await sender.Send(new GetDailyScheduleQuery(timeZone, scheduleDate, laborId), ct);

        return result.Match(Ok, Problem);
    }
}
