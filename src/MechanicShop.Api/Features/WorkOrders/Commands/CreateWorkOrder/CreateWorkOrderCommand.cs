using MechanicShop.Api.Domain.Common.Results;
using MechanicShop.Api.Domain.Workorders.Enums;
using MechanicShop.Api.Features.WorkOrders.Dtos;

using MediatR;

namespace MechanicShop.Api.Features.WorkOrders.Commands.CreateWorkOrder;

public sealed record CreateWorkOrderCommand(
    Spot Spot,
    Guid VehicleId,
    DateTimeOffset StartAt,
    List<Guid> RepairTaskIds,
    Guid? LaborId)
: IRequest<Result<WorkOrderDto>>;
