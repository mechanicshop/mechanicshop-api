using Asp.Versioning.Builder;

using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;
using MechanicShop.Api.Features.WorkOrders.Commands.DeleteWorkOrder;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.WorkOrders.Commands.DeleteWorkOrder;

public class DeleteWorkOrderEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        app.MapDelete("/api/v{version:apiVersion}/workorders/{WorkOrderId:guid}", async (Guid WorkOrderId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new DeleteWorkOrderCommand(WorkOrderId), ct);
            return result.Match(
                _ => Results.NoContent(),
                error => error.ToProblem());
        })
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(1.0)
        .RequireAuthorization("ManagerOnly")
        .WithName("DeleteWorkOrder")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
        .MapToApiVersion(1.0);
    }
}