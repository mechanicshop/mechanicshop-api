using Asp.Versioning.Builder;

using MechanicShop.Api.Domain.Identity;
using MechanicShop.Api.Domain.Workorders.Enums;
using MechanicShop.Api.DTOs.Requests.WorkOrders;
using MechanicShop.Api.Extensions;
using MechanicShop.Api.Features.WorkOrders.Commands.AssignLabor;
using MechanicShop.Api.Features.WorkOrders.Commands.CreateWorkOrder;
using MechanicShop.Api.Features.WorkOrders.Commands.DeleteWorkOrder;
using MechanicShop.Api.Features.WorkOrders.Commands.RelocateWorkOrder;
using MechanicShop.Api.Features.WorkOrders.Commands.UpdateOrderState;
using MechanicShop.Api.Features.WorkOrders.Commands.UpdateWorkOrderRepairTasks;
using MechanicShop.Api.Features.WorkOrders.Dtos;
using MechanicShop.Api.Features.WorkOrders.Queries.GetWorkOrderByIdQuery;
using MechanicShop.Api.Features.WorkOrders.Queries.GetWorkOrders;

using MediatR;

namespace MechanicShop.Api.Endpoints;

public static class WorkOrderEndpoints
{
    public static IEndpointRouteBuilder MapWorkOrderEndpoints(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        var group = app.MapGroup("/api/v{version:apiVersion}/workorders")
            .WithApiVersionSet(versionSet)
            .HasApiVersion(1.0)
            .RequireAuthorization()
            .MapToApiVersion(1.0);

        group.MapGet("/", async ([AsParameters] WorkOrderFilterRequest filterRequest,
            [AsParameters] PageRequest pageRequest, ISender sender, CancellationToken ct) =>
        {
            if (pageRequest.Page <= 0)
            {
                return Results.BadRequest("Page must be greater than 0");
            }

            if (pageRequest.PageSize <= 0 || pageRequest.PageSize > 100)
            {
                return Results.BadRequest("PageSize must be between 1 and 100");
            }

            var query = new GetWorkOrdersQuery(
                pageRequest.Page,
                pageRequest.PageSize,
                filterRequest.SearchTerm,
                filterRequest.SortColumn ?? "createdAt",
                filterRequest.SortDirection ?? "desc",
                filterRequest.State is not null ? (WorkOrderState)(int)filterRequest.State : null,
                filterRequest.VehicleId,
                filterRequest.LaborId,
                filterRequest.StartDateFrom,
                filterRequest.StartDateTo,
                filterRequest.EndDateFrom,
                filterRequest.EndDateTo,
                filterRequest.Spot is not null ? (Spot)(int)filterRequest.Spot : null);

            var result = await sender.Send(query, ct);

            return result.Match(Results.Ok, error => error.ToProblem());
        })
        .WithName("GetWorkOrders");

        group.MapGet("/completed", async ([AsParameters] WorkOrderFilterRequest filterRequest,
            [AsParameters] PageRequest pageRequest, ISender sender, CancellationToken ct) =>
        {
            if (pageRequest.Page <= 0)
            {
                return Results.BadRequest("Page must be greater than 0");
            }

            if (pageRequest.PageSize <= 0 || pageRequest.PageSize > 100)
            {
                return Results.BadRequest("PageSize must be between 1 and 100");
            }

            var query = new GetWorkOrdersQuery(
                pageRequest.Page,
                pageRequest.PageSize,
                filterRequest.SearchTerm,
                filterRequest.SortColumn ?? "createdAt",
                filterRequest.SortDirection ?? "desc",
                filterRequest.State is not null ? (WorkOrderState)(int)filterRequest.State : null,
                filterRequest.VehicleId,
                filterRequest.LaborId,
                filterRequest.StartDateFrom,
                filterRequest.StartDateTo,
                filterRequest.EndDateFrom,
                filterRequest.EndDateTo,
                filterRequest.Spot is not null ? (Spot)(int)filterRequest.Spot : null);

            var result = await sender.Send(query, ct);

            return result.Match(Results.Ok, error => error.ToProblem());
        })
        .WithName("GetCompletedWorkOrders");

        group.MapGet("/{workOrderId:guid}", async (Guid workOrderId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetWorkOrderByIdQuery(workOrderId), ct);

            return result.Match(Results.Ok, error => error.ToProblem());
        })
        .WithName("GetWorkOrderById");

        group.MapPost("/", async (CreateWorkOrderRequest request, ISender sender, CancellationToken ct) =>
        {
            var command = new CreateWorkOrderCommand(
                request.Spot,
                request.VehicleId,
                request.StartAtUtc,
                request.RepairTaskIds,
                request.LaborId);

            var result = await sender.Send(command, ct);

            return result.Match(
                wo => TypedResults.CreatedAtRoute(
                    value: result.Value,
                    routeName: "GetWorkOrderById",
                    routeValues: new { wo.WorkOrderId }),
                error => error.ToProblem());
        })
        .RequireAuthorization("ManagerOnly")
        .WithName("CreateWorkOrder");

        group.MapDelete("/{workOrderId:guid}", async (Guid workOrderId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new DeleteWorkOrderCommand(workOrderId), ct);

            return result.Match(_ => Results.NoContent(), error => error.ToProblem());
        })
        .RequireAuthorization("ManagerOnly")
        .WithName("DeleteWorkOrder");

        group.MapPut("/{workOrderId:guid}/labor", async (Guid workOrderId, AssignLaborRequest request, ISender sender, CancellationToken ct) =>
        {
            var command = new AssignLaborCommand(workOrderId, Guid.Parse(request.LaborId));

            var result = await sender.Send(command, ct);

            return result.Match(_ => Results.NoContent(), error => error.ToProblem());
        })
        .RequireAuthorization("ManagerOnly")
        .WithName("AssignLaborToWorkOrder");

        group.MapPut("/{workOrderId:guid}/relocation", async (Guid workOrderId, RelocateWorkOrderRequest request, ISender sender, CancellationToken ct) =>
        {
            var command = new RelocateWorkOrderCommand(workOrderId, request.NewStartAtUtc, (Spot)(int)request.NewSpot);

            var result = await sender.Send(command, ct);

            return result.Match(_ => Results.NoContent(), error => error.ToProblem());
        })
        .RequireAuthorization("ManagerOnly")
        .WithName("RelocateWorkOrder");

        group.MapPut("/{workOrderId:guid}/state", async (Guid workOrderId, UpdateWorkOrderStateRequest request, ISender sender, CancellationToken ct) =>
        {
            var command = new UpdateWorkOrderStateCommand(workOrderId, request.State);

            var result = await sender.Send(command, ct);

            return result.Match(_ => Results.NoContent(), error => error.ToProblem());
        })
        .RequireAuthorization(policy => policy.RequireRole(nameof(Role.Manager), nameof(Role.Labor)))
        .RequireAuthorization("SelfScopedWorkOrderAccess")
        .WithName("UpdateWorkOrderState");

        group.MapPut("/{workOrderId:guid}/repair-task", async (Guid workOrderId, ModifyRepairTaskRequest request, ISender sender, CancellationToken ct) =>
        {
            var command = new UpdateWorkOrderRepairTasksCommand(workOrderId, request.RepairTaskIds);

            var result = await sender.Send(command, ct);

            return result.Match(_ => Results.NoContent(), error => error.ToProblem());
        })
        .RequireAuthorization(policy => policy.RequireRole(nameof(Role.Manager)))
        .WithName("UpdateWorkOrderRepairTasks");

        return app;
    }
}
