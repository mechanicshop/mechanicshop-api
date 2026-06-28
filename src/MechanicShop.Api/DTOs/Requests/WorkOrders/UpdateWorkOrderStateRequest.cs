using MechanicShop.Api.Domain.Workorders.Enums;

namespace MechanicShop.Api.DTOs.Requests.WorkOrders;

public class UpdateWorkOrderStateRequest
{
    public WorkOrderState State { get; init; }
}