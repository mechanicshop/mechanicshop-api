namespace MechanicShop.Api.Common.Interfaces;

public interface IWorkOrderNotifier
{
    Task NotifyWorkOrdersChangedAsync(CancellationToken ct = default);
}
