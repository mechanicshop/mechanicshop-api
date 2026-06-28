using MechanicShop.Api.Domain.Common.Results;

using MediatR;

namespace MechanicShop.Api.Features.WorkOrders.Commands.UpdateWorkOrderRepairTasks;

public sealed record UpdateWorkOrderRepairTasksCommand(
    Guid WorkOrderId,
    Guid[] RepairTaskIds) : IRequest<Result<Updated>>;