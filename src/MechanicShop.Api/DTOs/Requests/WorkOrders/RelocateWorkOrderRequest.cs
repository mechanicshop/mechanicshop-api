using MechanicShop.Domain.Workorders.Enums;

namespace MechanicShop.Api.DTOs.Requests.WorkOrders;

public class RelocateWorkOrderRequest
{
    public DateTimeOffset NewStartAtUtc { get; init; }
    public Spot NewSpot { get; init; }
}
