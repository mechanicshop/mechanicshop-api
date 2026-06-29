using Asp.Versioning.Builder;

using MechanicShop.Api.Extensions;
using MechanicShop.Api.Features.Dashboard.Queries.GetWorkOrderStats;

using MediatR;

namespace MechanicShop.Api.Endpoints;

public static class DashboardEndpoints
{
    public static IEndpointRouteBuilder MapDashboardEndpoints(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        var group = app.MapGroup("/api/v{version:apiVersion}/dashboard")
            .WithApiVersionSet(versionSet)
            .HasApiVersion(1.0)
            .RequireAuthorization("ManagerOnly")
            .MapToApiVersion(1.0);

        group.MapGet("/stats", async (ISender sender, CancellationToken ct) =>
        {
            var date = DateOnly.FromDateTime(DateTime.UtcNow);
            var result = await sender.Send(new GetWorkOrderStatsQuery(date), ct);

            return result.Match(Results.Ok, error => error.ToProblem());
        })
        .WithName("GetWorkOrderStats");

        return app;
    }
}
