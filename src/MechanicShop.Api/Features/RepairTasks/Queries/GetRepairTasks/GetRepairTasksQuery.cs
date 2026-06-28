using MechanicShop.Api.Common.Interfaces;
using MechanicShop.Api.Domain.Common.Results;
using MechanicShop.Api.Features.RepairTasks.Dtos;

namespace MechanicShop.Api.Features.RepairTasks.Queries.GetRepairTasks;

public sealed record GetRepairTasksQuery() : ICachedQuery<Result<List<RepairTaskDto>>>
{
    public string CacheKey => "repair-tasks";

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);

    public string[] Tags => ["repair-tasks"];
}
