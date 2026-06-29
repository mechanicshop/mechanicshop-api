using Asp.Versioning.Builder;

using MechanicShop.Api.Domain.Workorders.Enums;
using MechanicShop.Api.DTOs.Requests.WorkOrders;
using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.WorkOrders.Commands.RelocateWorkOrder;

public class RelocateWorkOrderEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        app.MapPut("/api/v{version:apiVersion}/workorders/{workOrderId:guid}/relocation", async (Guid workOrderId, RelocateWorkOrderRequest request, ISender sender, CancellationToken ct) =>
        {
            var command = new RelocateWorkOrderCommand(
                workOrderId,
                request.NewStartAtUtc,
                (Spot)(int)request.NewSpot);

            var result = await sender.Send(command, ct);

            return result.Match(
                _ => Results.NoContent(),
                error => error.ToProblem());
        })
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(1.0)
        .RequireAuthorization("ManagerOnly")
        .WithName("RelocateWorkOrder")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
        .MapToApiVersion(1.0);
    }
}
