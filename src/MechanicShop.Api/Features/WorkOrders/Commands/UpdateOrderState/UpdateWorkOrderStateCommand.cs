using MechanicShop.Api.Domain.Common.Results;
using MechanicShop.Api.Domain.Workorders.Enums;

using MediatR;

namespace MechanicShop.Api.Features.WorkOrders.Commands.UpdateOrderState;

public sealed record UpdateWorkOrderStateCommand(
    Guid WorkOrderId,
    WorkOrderState State) : IRequest<Result<Updated>>;
