using Asp.Versioning.Builder;

using MechanicShop.Api.Domain.Identity;
using MechanicShop.Api.DTOs.Requests.WorkOrders;
using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.WorkOrders.Commands.UpdateOrderState;

public class UpdateWorkOrderStateEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        app.MapPut("/api/v{version:apiVersion}/workorders/{WorkOrderId:guid}/state", async (Guid WorkOrderId, UpdateWorkOrderStateRequest request, ISender sender, CancellationToken ct) =>
        {
            var command = new UpdateWorkOrderStateCommand(WorkOrderId, request.State);

            var result = await sender.Send(command, ct);

            return result.Match(
                _ => Results.NoContent(),
                error => error.ToProblem());
        })
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(1.0)
        .RequireAuthorization(policy => policy.RequireRole(nameof(Role.Manager), nameof(Role.Labor)))
        .RequireAuthorization("SelfScopedWorkOrderAccess")
        .WithName("UpdateWorkOrderState")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
        .MapToApiVersion(1.0);
    }
}
