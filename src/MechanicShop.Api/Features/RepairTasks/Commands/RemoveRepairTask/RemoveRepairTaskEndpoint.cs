using Asp.Versioning.Builder;

using MechanicShop.Api.Domain.Identity;
using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;
using MechanicShop.Api.Features.RepairTasks.Commands.RemoveRepairTask;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.RepairTasks.Commands.RemoveRepairTask;

public class RemoveRepairTaskEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        app.MapDelete("/api/v{version:apiVersion}/repair-tasks/{repairTaskId:guid}", async (Guid repairTaskId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new RemoveRepairTaskCommand(repairTaskId), ct);
            return result.Match(_ => Results.NoContent(), e => e.ToProblem());
        })
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(1.0)
        .RequireAuthorization(policy => policy.RequireRole(nameof(Role.Manager)))
        .WithName("RemoveRepairTask")
        .WithSummary("Removes a repair task.")
        .WithDescription("Deletes the specified repair task from the system.")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
        .MapToApiVersion(1.0);
    }
}