using MechanicShop.Api.Common.Interfaces;
using MechanicShop.Api.Domain.Workorders.Events;

using MediatR;

namespace MechanicShop.Api.Features.WorkOrders.EventHandlers;

public sealed class WorkOrderCollectionModifiedEventHandler(IWorkOrderNotifier notifier)
        : INotificationHandler<WorkOrderCollectionModified>
{
    private readonly IWorkOrderNotifier _notifier = notifier;

    public Task Handle(WorkOrderCollectionModified notification, CancellationToken ct) =>
        _notifier.NotifyWorkOrdersChangedAsync(ct);
}