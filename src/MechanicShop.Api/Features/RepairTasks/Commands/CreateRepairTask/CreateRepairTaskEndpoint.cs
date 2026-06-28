using Asp.Versioning.Builder;

using MechanicShop.Api.Domain.Identity;
using MechanicShop.Api.DTOs.Requests.RepairTasks;
using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;
using MechanicShop.Api.Features.RepairTasks.Dtos;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.RepairTasks.Commands.CreateRepairTask;

public class CreateRepairTaskEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        app.MapPost("/api/v{version:apiVersion}/repair-tasks", async ([FromBody] CreateRepairTaskRequest request, ISender sender, CancellationToken ct) =>
        {
            var parts = request.Parts
                .ConvertAll(p => new CreateRepairTaskPartCommand(p.Name, p.Cost, p.Quantity));

            var command = new CreateRepairTaskCommand(
                request.Name,
                request.LaborCost,
                request.EstimatedDurationInMins,
                parts);

            var result = await sender.Send(command, ct);

            return result.Match(
                r => Results.CreatedAtRoute("GetRepairTaskById", new { version = "1.0", repairTaskId = r.RepairTaskId }, r),
                e => e.ToProblem());
        })
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(1.0)
        .RequireAuthorization(policy => policy.RequireRole(nameof(Role.Manager)))
        .WithName("CreateRepairTask")
        .WithSummary("Creates a new repair task.")
        .WithDescription("Creates a repair task and optionally includes parts.")
        .Produces<RepairTaskDto>(StatusCodes.Status201Created)
        .Produces<ValidationProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
        .MapToApiVersion(1.0);
    }
}
