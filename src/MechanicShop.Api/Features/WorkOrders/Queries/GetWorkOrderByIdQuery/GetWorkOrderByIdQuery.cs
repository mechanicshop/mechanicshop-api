using MechanicShop.Api.Common.Interfaces;
using MechanicShop.Api.Domain.Common.Results;
using MechanicShop.Api.Features.WorkOrders.Dtos;

namespace MechanicShop.Api.Features.WorkOrders.Queries.GetWorkOrderByIdQuery;

public sealed record GetWorkOrderByIdQuery(Guid WorkOrderId) : ICachedQuery<Result<WorkOrderDto>>
{
    public string CacheKey => $"work-order:{WorkOrderId}";
    public string[] Tags => ["work-order"];
    public TimeSpan Expiration => TimeSpan.FromMinutes(10);
}