using Asp.Versioning.Builder;

using MechanicShop.Api.DTOs.Requests.WorkOrders;
using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;
using MechanicShop.Api.Features.WorkOrders.Dtos;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.WorkOrders.Commands.CreateWorkOrder;

public class CreateWorkOrderEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        app.MapPost("/api/v{version:apiVersion}/workorders", async (CreateWorkOrderRequest request, ISender sender, CancellationToken ct) =>
        {
            var command = new CreateWorkOrderCommand(
                request.Spot,
                request.VehicleId,
                request.StartAtUtc,
                request.RepairTaskIds,
                request.LaborId);

            var result = await sender.Send(command, ct);

            return result.Match(
                wo => TypedResults.CreatedAtRoute(
                    value: result.Value,
                    routeName: "GetWorkOrderById",
                    routeValues: new { wo.WorkOrderId }),
                error => error.ToProblem());
        })
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(1.0)
        .RequireAuthorization("ManagerOnly")
        .WithName("CreateWorkOrder")
        .Produces<WorkOrderDto>(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
        .MapToApiVersion(1.0);
    }
}
