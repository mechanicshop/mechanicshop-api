using MechanicShop.Api.Domain.Common.Results;
using MechanicShop.Api.Domain.RepairTasks.Enums;

using MediatR;

namespace MechanicShop.Api.Features.RepairTasks.Commands.UpdateRepairTask;

public sealed record UpdateRepairTaskCommand(
    Guid RepairTaskId,
    string Name,
    decimal LaborCost,
    RepairDurationInMinutes EstimatedDurationInMins,
    List<UpdateRepairTaskPartCommand> Parts
) : IRequest<Result<Updated>>;
