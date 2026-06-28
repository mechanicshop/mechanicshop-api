using Asp.Versioning.Builder;

using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;
using MechanicShop.Api.Features.RepairTasks.Dtos;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.RepairTasks.Queries.GetRepairTasks;

public class GetRepairTasksEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        app.MapGet("/api/v{version:apiVersion}/repair-tasks", async (ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetRepairTasksQuery(), ct);
            return result.Match(Results.Ok, e => e.ToProblem());
        })
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(1.0)
        .RequireAuthorization()
        .WithName("GetRepairTasks")
        .WithSummary("Retrieves all repair tasks.")
        .WithDescription("Returns a list of all repair tasks available in the system.")
        .Produces<List<RepairTaskDto>>()
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
        .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)))
        .MapToApiVersion(1.0);
    }
}
