using MechanicShop.Api.Domain.Common;

namespace MechanicShop.Api.Domain.Workorders.Events;

public sealed class WorkOrderCompleted : DomainEvent
{
    public Guid WorkOrderId { get; init; }
}
