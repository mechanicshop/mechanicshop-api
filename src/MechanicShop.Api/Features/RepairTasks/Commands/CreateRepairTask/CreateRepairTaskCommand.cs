using MechanicShop.Api.Domain.Common.Results;
using MechanicShop.Api.Domain.RepairTasks.Enums;
using MechanicShop.Api.Features.RepairTasks.Dtos;

using MediatR;

namespace MechanicShop.Api.Features.RepairTasks.Commands.CreateRepairTask;

public sealed record CreateRepairTaskCommand(
    string? Name,
    decimal LaborCost,
    RepairDurationInMinutes? EstimatedDurationInMins,
    List<CreateRepairTaskPartCommand> Parts
) : IRequest<Result<RepairTaskDto>>;