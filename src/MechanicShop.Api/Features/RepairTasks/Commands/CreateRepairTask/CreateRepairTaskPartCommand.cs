using MechanicShop.Api.Domain.Common.Results;

using MediatR;

namespace MechanicShop.Api.Features.RepairTasks.Commands.CreateRepairTask;

public sealed record CreateRepairTaskPartCommand(
    string Name,
    decimal Cost,
    int Quantity
) : IRequest<Result<Success>>;