using MechanicShop.Api.Common.Interfaces;

using Microsoft.AspNetCore.SignalR;

namespace MechanicShop.Api.Infrastructure.RealTime;

public sealed class SignalRWorkOrderNotifier(IHubContext<WorkOrderHub> hubContext) : IWorkOrderNotifier
{
    public Task NotifyWorkOrdersChangedAsync(CancellationToken ct = default) =>
        hubContext.Clients.All.SendAsync("WorkOrdersChanged", cancellationToken: ct);
}