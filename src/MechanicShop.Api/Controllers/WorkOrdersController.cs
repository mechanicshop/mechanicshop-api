using Asp.Versioning;

using MechanicShop.Api.DTOs.Requests.WorkOrders;
using MechanicShop.Application.Features.WorkOrders.Commands.AssignLabor;
using MechanicShop.Application.Features.WorkOrders.Commands.CreateWorkOrder;
using MechanicShop.Application.Features.WorkOrders.Commands.DeleteWorkOrder;
using MechanicShop.Application.Features.WorkOrders.Commands.RelocateWorkOrder;
using MechanicShop.Application.Features.WorkOrders.Commands.UpdateOrderState;
using MechanicShop.Application.Features.WorkOrders.Commands.UpdateWorkOrderRepairTasks;
using MechanicShop.Application.Features.WorkOrders.Queries.GetWorkOrderByIdQuery;
using MechanicShop.Application.Features.WorkOrders.Queries.GetWorkOrders;
using MechanicShop.Domain.Workorders.Enums;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Controllers;

[Route("api/v{version:apiVersion}/workorders")]
[ApiVersion("1.0")]
[Authorize]
public class WorkOrdersController(ISender sender) : ApiController
{
    [HttpGet]
    public async Task<ActionResult> GetWorkOrders(
        [AsParameters] WorkOrderFilterRequest filterRequest,
        [AsParameters] PageRequest pageRequest,
        CancellationToken ct)
    {
        if (pageRequest.Page <= 0)
        {
            return BadRequest("Page must be greater than 0");
        }

        if (pageRequest.PageSize <= 0 || pageRequest.PageSize > 100)
        {
            return BadRequest("PageSize must be between 1 and 100");
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
            filterRequest.StartDate,
            filterRequest.EndDate,
            filterRequest.Spot is not null ? (Spot)(int)filterRequest.Spot : null);

        var result = await sender.Send(query, ct);

        return result.Match(Ok, Problem);
    }

    [HttpGet("{workOrderId:guid}")]
    public async Task<ActionResult> GetWorkOrderById(Guid workOrderId, CancellationToken ct)
    {
        var result = await sender.Send(new GetWorkOrderByIdQuery(workOrderId), ct);

        return result.Match(Ok, Problem);
    }

    [HttpPost]
    [Authorize("ManagerOnly")]
    public async Task<ActionResult> CreateWorkOrder(CreateWorkOrderRequest request, CancellationToken ct)
    {
        var command = new CreateWorkOrderCommand(
            request.Spot,
            request.VehicleId,
            request.StartAtUtc,
            request.RepairTaskIds,
            request.LaborId);

        var result = await sender.Send(command, ct);

        return result.Match(
            wo => CreatedAtRoute("GetWorkOrderById", new { wo.WorkOrderId }, wo),
            Problem);
    }

    [HttpDelete("{workOrderId:guid}")]
    [Authorize("ManagerOnly")]
    public async Task<ActionResult> DeleteWorkOrder(Guid workOrderId, CancellationToken ct)
    {
        var result = await sender.Send(new DeleteWorkOrderCommand(workOrderId), ct);

        return result.Match(_ => NoContent(), Problem);
    }

    [HttpPut("{workOrderId:guid}/labor")]
    [Authorize("ManagerOnly")]
    public async Task<ActionResult> AssignLabor(Guid workOrderId, AssignLaborRequest request, CancellationToken ct)
    {
        var command = new AssignLaborCommand(workOrderId, Guid.Parse(request.LaborId));

        var result = await sender.Send(command, ct);

        return result.Match(_ => NoContent(), Problem);
    }

    [HttpPut("{workOrderId:guid}/relocation")]
    [Authorize("ManagerOnly")]
    public async Task<ActionResult> RelocateWorkOrder(Guid workOrderId, RelocateWorkOrderRequest request, CancellationToken ct)
    {
        var command = new RelocateWorkOrderCommand(
            workOrderId,
            request.NewStartAtUtc,
            (Spot)(int)request.NewSpot);

        var result = await sender.Send(command, ct);

        return result.Match(_ => NoContent(), Problem);
    }

    [HttpPut("{workOrderId:guid}/state")]
    [Authorize(policy: "SelfScopedWorkOrderAccess")]
    public async Task<ActionResult> UpdateWorkOrderState(Guid workOrderId, UpdateWorkOrderStateRequest request, CancellationToken ct)
    {
        var command = new UpdateWorkOrderStateCommand(workOrderId, request.State);

        var result = await sender.Send(command, ct);

        return result.Match(_ => NoContent(), Problem);
    }

    [HttpPut("{workOrderId:guid}/repair-task")]
    [Authorize("ManagerOnly")]
    public async Task<ActionResult> UpdateWorkOrderRepairTasks(Guid workOrderId, ModifyRepairTaskRequest request, CancellationToken ct)
    {
        var command = new UpdateWorkOrderRepairTasksCommand(workOrderId, request.RepairTaskIds);

        var result = await sender.Send(command, ct);

        return result.Match(_ => NoContent(), Problem);
    }
}
