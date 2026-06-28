using Asp.Versioning.Builder;

using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;
using MechanicShop.Api.Features.RepairTasks.Dtos;
using MechanicShop.Api.Features.RepairTasks.Queries.GetRepairTaskById;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.RepairTasks.Queries.GetRepairTaskById;

public class GetRepairTaskByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        app.MapGet("/api/v{version:apiVersion}/repair-tasks/{repairTaskId:guid}", async (Guid repairTaskId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetRepairTaskByIdQuery(repairTaskId), ct);
            return result.Match(Results.Ok, e => e.ToProblem());
        })
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(1.0)
        .RequireAuthorization()
        .WithName("GetRepairTaskById")
        .WithSummary("Retrieves a repair task by ID.")
        .WithDescription("Returns detailed information for the specified repair task if it exists.")
        .Produces<RepairTaskDto>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
        .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)))
        .MapToApiVersion(1.0);
    }
}