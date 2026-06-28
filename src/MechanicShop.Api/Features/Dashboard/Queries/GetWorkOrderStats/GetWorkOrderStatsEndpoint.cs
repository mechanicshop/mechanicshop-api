using Asp.Versioning.Builder;

using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;
using MechanicShop.Api.Features.Dashboard.Dtos;
using MechanicShop.Api.Features.Dashboard.Queries.GetWorkOrderStats;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.Dashboard.Queries.GetWorkOrderStats;

public class GetWorkOrderStatsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        app.MapGet("/api/v{version:apiVersion}/dashboard/stats", async ([AsParameters] DashboardStatsQuery queryParams, ISender sender, CancellationToken ct) =>
        {
            var date = queryParams.Date ?? DateOnly.FromDateTime(DateTime.UtcNow);
            var result = await sender.Send(new GetWorkOrderStatsQuery(date), ct);

            return result.Match(
                Results.Ok,
                error => error.ToProblem());
        })
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(1.0)
        .RequireAuthorization("ManagerOnly")
        .WithName("GetWorkOrderStats")
        .WithSummary("Get today's work order stats")
        .Produces<TodayWorkOrderStatsDto>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
        .MapToApiVersion(1.0);
    }
}

public sealed record DashboardStatsQuery([FromQuery] DateOnly? Date);