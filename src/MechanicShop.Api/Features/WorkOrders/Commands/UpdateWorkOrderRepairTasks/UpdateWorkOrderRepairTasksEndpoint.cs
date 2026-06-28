using Asp.Versioning.Builder;

using MechanicShop.Api.Domain.Identity;
using MechanicShop.Api.DTOs.Requests.WorkOrders;
using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.WorkOrders.Commands.UpdateWorkOrderRepairTasks;

public class UpdateWorkOrderRepairTasksEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        app.MapPut("/api/v{version:apiVersion}/workorders/{WorkOrderId:guid}/repair-task", async (Guid WorkOrderId, ModifyRepairTaskRequest request, ISender sender, CancellationToken ct) =>
        {
            var command = new UpdateWorkOrderRepairTasksCommand(WorkOrderId, request.RepairTaskIds);

            var result = await sender.Send(command, ct);

            return result.Match(
                _ => Results.NoContent(),
                error => error.ToProblem());
        })
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(1.0)
        .RequireAuthorization(policy => policy.RequireRole(nameof(Role.Manager)))
        .WithName("UpdateWorkOrderRepairTasks")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
        .MapToApiVersion(1.0);
    }
}
