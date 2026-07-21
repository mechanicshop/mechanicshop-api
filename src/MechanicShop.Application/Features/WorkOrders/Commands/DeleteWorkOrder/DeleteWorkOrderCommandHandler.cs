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
    private readonly ILogger<DeleteWorkOrderCommandHandler> _logger = logger;
    private readonly IAppDbContext _context = context;
    private readonly HybridCache _cache = cache;
    private readonly TimeProvider _dateTime = dateTime;

    public async Task<Result<Deleted>> Handle(DeleteWorkOrderCommand command, CancellationToken ct)
    {
        var workOrder = await _context.WorkOrders
            .FirstOrDefaultAsync(a => a.Id == command.WorkOrderId, ct);

        if (workOrder is null)
        {
            _logger.LogError("WorkOrder with Id '{WorkOrderId}' does not exist.", command.WorkOrderId);

            return ApplicationErrors.WorkOrderNotFound;
        }

        var deleteResult = workOrder.Delete(_dateTime.GetUtcNow());

        if (deleteResult.IsError)
        {
            _logger.LogError(
                "Deletion failed: only 'Scheduled' or 'Confirmed' WorkOrders can be deleted. Current status: {Status}",
                workOrder.State);

            return deleteResult.Errors;
        }

        await _context.SaveChangesAsync(ct);

        workOrder.AddDomainEvent(new WorkOrderCollectionModified());

        await _cache.RemoveByTagAsync("work-order", ct);

        return Result.Deleted;
    }
}
