using Asp.Versioning.Builder;

using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;
using MechanicShop.Api.Features.WorkOrders.Dtos;
using MechanicShop.Api.Features.WorkOrders.Queries.GetWorkOrderByIdQuery;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.WorkOrders.Queries.GetWorkOrderByIdQuery;

public class GetWorkOrderByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        app.MapGet("/api/v{version:apiVersion}/workorders/{WorkOrderId:guid}", async (Guid WorkOrderId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetWorkOrderByIdQuery(WorkOrderId), ct);
            return result.Match(Results.Ok, error => error.ToProblem());
        })
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(1.0)
        .RequireAuthorization()
        .WithName("GetWorkOrderById")
        .Produces<WorkOrderDto>()
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
        .MapToApiVersion(1.0);
    }
}