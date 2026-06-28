namespace MechanicShop.Api.DTOs.Requests.WorkOrders;

public class ModifyRepairTaskRequest
{
    public Guid[] RepairTaskIds { get; set; } = [];
}