using Asp.Versioning.Builder;

using MechanicShop.Api.DTOs.Requests.WorkOrders;
using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.WorkOrders.Commands.AssignLabor;

public class AssignLaborEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        app.MapPut("/api/v{version:apiVersion}/workorders/{workOrderId:guid}/labor", async (Guid workOrderId, AssignLaborRequest request, ISender sender, CancellationToken ct) =>
        {
            var command = new AssignLaborCommand(workOrderId, Guid.Parse(request.LaborId));

            var result = await sender.Send(command, ct);

            return result.Match(
                _ => Results.NoContent(),
                error => error.ToProblem());
        })
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(1.0)
        .RequireAuthorization("ManagerOnly")
        .WithName("AssignLaborToWorkOrder")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
        .MapToApiVersion(1.0);
    }
}
