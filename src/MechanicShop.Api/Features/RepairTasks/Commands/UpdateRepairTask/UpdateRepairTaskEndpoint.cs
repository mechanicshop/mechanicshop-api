using Asp.Versioning.Builder;

using MechanicShop.Api.Domain.Identity;
using MechanicShop.Api.DTOs.Requests.RepairTasks;
using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;
using MechanicShop.Api.Features.RepairTasks.Dtos;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.RepairTasks.Commands.UpdateRepairTask;

public class UpdateRepairTaskEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        app.MapPut("/api/v{version:apiVersion}/repair-tasks/{repairTaskId:guid}", async (Guid repairTaskId, [FromBody] UpdateRepairTaskRequest request, ISender sender, CancellationToken ct) =>
        {
            var parts = request.Parts
                .ConvertAll(p => new UpdateRepairTaskPartCommand(p.PartId, p.Name, p.Cost, p.Quantity));

            var command = new UpdateRepairTaskCommand(
                repairTaskId,
                request.Name,
                request.LaborCost,
                request.EstimatedDurationInMins,
                parts);

            var result = await sender.Send(command, ct);
            return result.Match(Results.Ok, e => e.ToProblem());
        })
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(1.0)
        .RequireAuthorization(policy => policy.RequireRole(nameof(Role.Manager)))
        .WithName("UpdateRepairTask")
        .WithSummary("Updates an existing repair task.")
        .WithDescription("Updates a repair task and its associated parts.")
        .Produces<RepairTaskDto>()
        .Produces<ValidationProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
        .MapToApiVersion(1.0);
    }
}
