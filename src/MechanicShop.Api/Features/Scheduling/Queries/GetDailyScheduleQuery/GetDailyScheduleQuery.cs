using MechanicShop.Api.Common.Interfaces;
using MechanicShop.Api.Domain.Common.Results;
using MechanicShop.Api.Features.Scheduling.Dtos;

namespace MechanicShop.Api.Features.Scheduling.Queries.GetDailyScheduleQuery;

public sealed record GetDailyScheduleQuery(
    TimeZoneInfo TimeZone,
    DateOnly ScheduleDate,
    Guid? LaborId = null) : ICachedQuery<Result<ScheduleDto>>
{
    public string CacheKey => $"work-order:{ScheduleDate:yyyy-MM-dd}:labor={LaborId?.ToString() ?? "-"}";
    public string[] Tags => ["work-order"];
    public TimeSpan Expiration => TimeSpan.FromMinutes(10);
}