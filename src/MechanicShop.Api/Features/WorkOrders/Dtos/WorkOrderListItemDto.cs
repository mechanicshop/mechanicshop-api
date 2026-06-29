using MechanicShop.Api.Domain.Workorders.Enums;
using MechanicShop.Api.Features.Customers.Dtos;

namespace MechanicShop.Api.Features.WorkOrders.Dtos;

public class WorkOrderListItemDto
{
    public Guid WorkOrderId { get; set; }
    public Guid? InvoiceId { get; set; }
    public VehicleDto Vehicle { get; set; } = null!;
    public string? Customer { get; set; }
    public string? Labor { get; set; }
    public WorkOrderState State { get; set; }
    public Spot Spot { get; set; }
    public DateTimeOffset StartAtUtc { get; set; }
    public DateTimeOffset EndAtUtc { get; set; }
    public List<string> RepairTasks { get; set; } = [];
}
