using Asp.Versioning.Builder;

using MechanicShop.Api.Extensions;
using MechanicShop.Application.Features.Scheduling.Queries.GetDailyScheduleQuery;
using MechanicShop.Domain.Identity;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Endpoints;

public static class SchedulingEndpoints
{
    public static IEndpointRouteBuilder MapSchedulingEndpoints(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        var group = app.MapGroup("/api/v{version:apiVersion}/workorders")
            .WithApiVersionSet(versionSet)
            .HasApiVersion(1.0)
            .RequireAuthorization(policy => policy.RequireRole(nameof(Role.Manager), nameof(Role.Labor)))
            .MapToApiVersion(1.0);

        group.MapGet("/schedule/{date}", async (
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

            return result.Match<IResult>(Results.Ok, error => error.ToProblem());
        })
        .WithName("GetDailySchedule");

        return app;
    }
}
