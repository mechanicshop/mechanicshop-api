using Asp.Versioning.Builder;

using MechanicShop.Api.Domain.Identity;
using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;
using MechanicShop.Api.Features.Scheduling.Dtos;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.Scheduling.Queries.GetDailyScheduleQuery;

public class GetDailyScheduleEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        app.MapGet("/api/v{version:apiVersion}/workorders/schedule/{date}", async (
            DateOnly? date,
            [FromQuery] Guid? laborId,
            [FromHeader(Name = "X-TimeZone")] string? tz,
            ISender sender,
            CancellationToken ct) =>
        {
            if (string.IsNullOrWhiteSpace(tz))
            {
                return Results.Problem(
                    detail: "Missing time zone in 'X-TimeZone' header.",
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Time Zone Required");
            }

            TimeZoneInfo timeZone;

            try
            {
                timeZone = TimeZoneInfo.FindSystemTimeZoneById(tz);
            }
            catch
            {
                return Results.Problem(
                    detail: $"Invalid or unknown time zone: '{tz}'.",
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Invalid Time Zone");
            }

            var scheduleDate = date ?? DateOnly.FromDateTime(DateTime.UtcNow);

            var result = await sender.Send(new GetDailyScheduleQuery(timeZone, scheduleDate, laborId), ct);

            return result.Match<IResult>(
                Results.Ok,
                error => error.ToProblem());
        })
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(1.0)
        .RequireAuthorization(policy => policy.RequireRole(nameof(Role.Manager), nameof(Role.Labor)))
        .WithName("GetDailySchedule")
        .Produces<ScheduleDto>()
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
        .MapToApiVersion(1.0);
    }
}
