using MechanicShop.Api.Domain.Common.Results;
using MechanicShop.Api.Domain.Workorders.Enums;

using MediatR;

namespace MechanicShop.Api.Features.WorkOrders.Commands.RelocateWorkOrder;

public sealed record RelocateWorkOrderCommand(
    Guid WorkOrderId,
    DateTimeOffset NewStartAt,
    Spot NewSpot) : IRequest<Result<Updated>>;