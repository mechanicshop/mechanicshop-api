using MechanicShop.Api.Domain.Common.Results;

using MediatR;

namespace MechanicShop.Api.Features.WorkOrders.Commands.DeleteWorkOrder;

public sealed record DeleteWorkOrderCommand(Guid WorkOrderId) : IRequest<Result<Deleted>>;
