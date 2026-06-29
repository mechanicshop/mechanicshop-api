namespace MechanicShop.Api.DTOs.Requests.WorkOrders;

public class ModifyRepairTaskRequest
{
    public List<Guid> RepairTaskIds { get; set; } = [];
}
