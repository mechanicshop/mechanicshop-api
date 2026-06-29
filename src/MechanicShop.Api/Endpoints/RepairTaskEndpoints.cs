using Asp.Versioning.Builder;

using MechanicShop.Api.Domain.Identity;
using MechanicShop.Api.DTOs.Requests.RepairTasks;
using MechanicShop.Api.Extensions;
using MechanicShop.Api.Features.RepairTasks.Commands.CreateRepairTask;
using MechanicShop.Api.Features.RepairTasks.Commands.RemoveRepairTask;
using MechanicShop.Api.Features.RepairTasks.Commands.UpdateRepairTask;
using MechanicShop.Api.Features.RepairTasks.Queries.GetRepairTaskById;
using MechanicShop.Api.Features.RepairTasks.Queries.GetRepairTasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Endpoints;

public static class RepairTaskEndpoints
{
    public static IEndpointRouteBuilder MapRepairTaskEndpoints(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        var group = app.MapGroup("/api/v{version:apiVersion}/repair-tasks")
            .WithApiVersionSet(versionSet)
            .HasApiVersion(1.0)
            .RequireAuthorization()
            .MapToApiVersion(1.0);

        group.MapGet("/", async (ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetRepairTasksQuery(), ct);

            return result.Match(Results.Ok, e => e.ToProblem());
        })
        .WithName("GetRepairTasks")
        .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)));

        group.MapGet("/{repairTaskId:guid}", async (Guid repairTaskId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetRepairTaskByIdQuery(repairTaskId), ct);

            return result.Match(Results.Ok, e => e.ToProblem());
        })
        .WithName("GetRepairTaskById")
        .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)));

        group.MapPost("/", async ([FromBody] CreateRepairTaskRequest request, ISender sender, CancellationToken ct) =>
        {
            var parts = request.Parts
                .ConvertAll(p => new CreateRepairTaskPartCommand(p.Name, p.Cost, p.Quantity));

            var command = new CreateRepairTaskCommand(request.Name, request.LaborCost, request.EstimatedDurationInMins, parts);

            var result = await sender.Send(command, ct);

            return result.Match(
                r => Results.CreatedAtRoute("GetRepairTaskById", new { version = "1.0", repairTaskId = r.RepairTaskId }, r),
                e => e.ToProblem());
        })
        .RequireAuthorization(policy => policy.RequireRole(nameof(Role.Manager)))
        .WithName("CreateRepairTask");

        group.MapPut("/{repairTaskId:guid}", async (Guid repairTaskId, [FromBody] UpdateRepairTaskRequest request, ISender sender, CancellationToken ct) =>
        {
            var parts = request.Parts
                .ConvertAll(p => new UpdateRepairTaskPartCommand(p.PartId, p.Name, p.Cost, p.Quantity));

            var command = new UpdateRepairTaskCommand(repairTaskId, request.Name, request.LaborCost, request.EstimatedDurationInMins, parts);

            var result = await sender.Send(command, ct);

            return result.Match(Results.Ok, e => e.ToProblem());
        })
        .RequireAuthorization(policy => policy.RequireRole(nameof(Role.Manager)))
        .WithName("UpdateRepairTask");

        group.MapDelete("/{repairTaskId:guid}", async (Guid repairTaskId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new RemoveRepairTaskCommand(repairTaskId), ct);

            return result.Match(_ => Results.NoContent(), e => e.ToProblem());
        })
        .RequireAuthorization(policy => policy.RequireRole(nameof(Role.Manager)))
        .WithName("RemoveRepairTask");

        return app;
    }
}
