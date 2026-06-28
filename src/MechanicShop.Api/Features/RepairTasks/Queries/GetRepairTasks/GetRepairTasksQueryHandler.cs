using MechanicShop.Api.Common.Interfaces;
using MechanicShop.Api.Domain.Common.Results;
using MechanicShop.Api.Features.RepairTasks.Dtos;
using MechanicShop.Api.Features.RepairTasks.Mappers;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace MechanicShop.Api.Features.RepairTasks.Queries.GetRepairTasks;

public class GetRepairTasksQueryHandler(IAppDbContext context)
    : IRequestHandler<GetRepairTasksQuery, Result<List<RepairTaskDto>>>
{
    public async Task<Result<List<RepairTaskDto>>> Handle(GetRepairTasksQuery query, CancellationToken ct)
    {
        var repairTasks = await context.RepairTasks.Include(rt => rt.Parts).AsNoTracking().ToListAsync(ct);

        return repairTasks.ToDtos();
    }
}