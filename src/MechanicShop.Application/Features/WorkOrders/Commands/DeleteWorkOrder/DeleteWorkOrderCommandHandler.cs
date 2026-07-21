using MechanicShop.Application.Common.Errors;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.Workorders.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

using Microsoft.Extensions.Logging;
namespace MechanicShop.Application.Features.WorkOrders.Commands.DeleteWorkOrder;

public class DeleteWorkOrderCommandHandler(
    ILogger<DeleteWorkOrderCommandHandler> logger,
    IAppDbContext context,
    HybridCache cache,
    TimeProvider dateTime
    )
    : IRequestHandler<DeleteWorkOrderCommand, Result<Deleted>>
{
    public async Task<Result<Deleted>> Handle(DeleteWorkOrderCommand command, CancellationToken ct)
    {
        var workOrder = await context.WorkOrders
            .FirstOrDefaultAsync(a => a.Id == command.WorkOrderId, ct);

        if (workOrder is null)
        {
            logger.LogError("WorkOrder with Id '{WorkOrderId}' does not exist.", command.WorkOrderId);

            return ApplicationErrors.WorkOrderNotFound;
        }

        var deleteResult = workOrder.Delete(dateTime.GetUtcNow());

        if (deleteResult.IsError)
        {
            logger.LogError(
                "Deletion failed: only 'Scheduled' or 'Confirmed' WorkOrders can be deleted. Current status: {Status}",
                workOrder.State);

            return deleteResult.Errors;
        }

        await context.SaveChangesAsync(ct);

        workOrder.AddDomainEvent(new WorkOrderCollectionModified());

        await cache.RemoveByTagAsync("work-order", ct);

        return Result.Deleted;
    }
}
