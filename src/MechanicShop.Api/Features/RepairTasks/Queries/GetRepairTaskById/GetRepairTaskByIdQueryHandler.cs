using MechanicShop.Api.Common.Errors;
using MechanicShop.Api.Common.Interfaces;
using MechanicShop.Api.Domain.Common.Results;
using MechanicShop.Api.Features.RepairTasks.Dtos;
using MechanicShop.Api.Features.RepairTasks.Mappers;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace MechanicShop.Api.Features.RepairTasks.Queries.GetRepairTaskById;

public class GetRepairTaskByIdQueryHandler(
    ILogger<GetRepairTaskByIdQueryHandler> logger,
    IAppDbContext context
    )
    : IRequestHandler<GetRepairTaskByIdQuery, Result<RepairTaskDto>>
{
    public async Task<Result<RepairTaskDto>> Handle(GetRepairTaskByIdQuery query, CancellationToken ct)
    {
        var repairTask = await context.RepairTasks.AsNoTracking().Include(c => c.Parts)
                                     .FirstOrDefaultAsync(c => c.Id == query.RepairTaskId, ct);

        if (repairTask is null)
        {
            logger.LogWarning("Repair task with id {RepairTaskId} was not found", query.RepairTaskId);

            return ApplicationErrors.RepairTaskNotFound;
        }

        return repairTask.ToDto();
    }
}
