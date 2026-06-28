using MechanicShop.Api.Domain.Common.Results;
using MechanicShop.Api.Features.Dashboard.Dtos;

using MediatR;

namespace MechanicShop.Api.Features.Dashboard.Queries.GetWorkOrderStats;

public sealed record GetWorkOrderStatsQuery(DateOnly Date) : IRequest<Result<TodayWorkOrderStatsDto>>;