using Asp.Versioning;

using MechanicShop.Api.DTOs.Requests.RepairTasks;
using MechanicShop.Application.Features.RepairTasks.Commands.CreateRepairTask;
using MechanicShop.Application.Features.RepairTasks.Commands.RemoveRepairTask;
using MechanicShop.Application.Features.RepairTasks.Commands.UpdateRepairTask;
using MechanicShop.Application.Features.RepairTasks.Queries.GetRepairTaskById;
using MechanicShop.Application.Features.RepairTasks.Queries.GetRepairTasks;
using MechanicShop.Domain.Identity;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Controllers;

[Route("api/v{version:apiVersion}/repair-tasks")]
[ApiVersion("1.0")]
[Authorize]
public class RepairTasksController : ApiController
{
    private readonly ISender _sender;

    public RepairTasksController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult> GetRepairTasks(CancellationToken ct)
    {
        var result = await _sender.Send(new GetRepairTasksQuery(), ct);

        return result.Match<ActionResult>(Ok, Problem);
    }

    [HttpGet("{repairTaskId:guid}")]
    public async Task<ActionResult> GetRepairTaskById(Guid repairTaskId, CancellationToken ct)
    {
        var result = await _sender.Send(new GetRepairTaskByIdQuery(repairTaskId), ct);

        return result.Match<ActionResult>(Ok, Problem);
    }

    [HttpPost]
    [Authorize(Roles = nameof(Role.Manager))]
    public async Task<ActionResult> CreateRepairTask(CreateRepairTaskRequest request, CancellationToken ct)
    {
        var parts = request.Parts
            .ConvertAll(p => new CreateRepairTaskPartCommand(p.Name, p.Cost, p.Quantity));

        var command = new CreateRepairTaskCommand(request.Name, request.LaborCost, request.EstimatedDurationInMins, parts);

        var result = await _sender.Send(command, ct);

        return result.Match<ActionResult>(
            r => CreatedAtRoute("GetRepairTaskById", new { version = "1.0", repairTaskId = r.RepairTaskId }, r),
            Problem);
    }

    [HttpPut("{repairTaskId:guid}")]
    [Authorize(Roles = nameof(Role.Manager))]
    public async Task<ActionResult> UpdateRepairTask(Guid repairTaskId, UpdateRepairTaskRequest request, CancellationToken ct)
    {
        var parts = request.Parts
            .ConvertAll(p => new UpdateRepairTaskPartCommand(p.PartId, p.Name, p.Cost, p.Quantity));

        var command = new UpdateRepairTaskCommand(repairTaskId, request.Name, request.LaborCost, request.EstimatedDurationInMins, parts);

        var result = await _sender.Send(command, ct);

        return result.Match<ActionResult>(value => Ok(value), Problem);
    }

    [HttpDelete("{repairTaskId:guid}")]
    [Authorize(Roles = nameof(Role.Manager))]
    public async Task<ActionResult> RemoveRepairTask(Guid repairTaskId, CancellationToken ct)
    {
        var result = await _sender.Send(new RemoveRepairTaskCommand(repairTaskId), ct);

        return result.Match<ActionResult>(_ => NoContent(), Problem);
    }
}
