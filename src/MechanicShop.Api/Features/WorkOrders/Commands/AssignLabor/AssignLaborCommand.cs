using MechanicShop.Api.Domain.Common.Results;

using MediatR;

namespace MechanicShop.Api.Features.WorkOrders.Commands.AssignLabor;

public sealed record AssignLaborCommand(Guid WorkOrderId, Guid LaborId) : IRequest<Result<Updated>>;
