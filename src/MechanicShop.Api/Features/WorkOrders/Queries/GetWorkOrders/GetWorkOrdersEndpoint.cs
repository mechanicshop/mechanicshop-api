using Asp.Versioning.Builder;

using MechanicShop.Api.Common.Models;
using MechanicShop.Api.Domain.Workorders.Enums;
using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;
using MechanicShop.Api.Features.WorkOrders.Dtos;
using MechanicShop.Api.Features.WorkOrders.Queries.GetWorkOrders;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.WorkOrders.Queries.GetWorkOrders;

public class GetWorkOrdersEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        // 1. Standard GetWorkOrders list route
        app.MapGet("/api/v{version:apiVersion}/workorders", GetWorkOrdersHandler)
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(1.0)
        .RequireAuthorization()
        .WithName("GetWorkOrders")
        .Produces<PaginatedList<WorkOrderListItemDto>>()
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
        .MapToApiVersion(1.0);

        // 2. Completed list route
        app.MapGet("/api/v{version:apiVersion}/workorders/completed", GetWorkOrdersHandler)
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(1.0)
        .RequireAuthorization()
        .WithName("GetCompletedWorkOrders")
        .Produces<PaginatedList<WorkOrderDto>>()
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
        .MapToApiVersion(1.0);
    }

    private static async Task<IResult> GetWorkOrdersHandler(
        ISender sender,
        [AsParameters] WorkOrderFilterRequest filterRequest,
        [AsParameters] PageRequest pageRequest,
        CancellationToken ct)
    {
        if (pageRequest.Page <= 0)
        {
            return Results.BadRequest("Page must be greater than 0");
        }

        if (pageRequest.PageSize <= 0 || pageRequest.PageSize > 100)
        {
            return Results.BadRequest("PageSize must be between 1 and 100");
        }

        var query = new GetWorkOrdersQuery(
            pageRequest.Page,
            pageRequest.PageSize,
            filterRequest.SearchTerm,
            filterRequest.SortColumn,
            filterRequest.SortDirection,
            filterRequest.State is not null ? (WorkOrderState)(int)filterRequest.State : null,
            filterRequest.VehicleId,
            filterRequest.LaborId,
            filterRequest.StartDateFrom,
            filterRequest.StartDateTo,
            filterRequest.EndDateFrom,
            filterRequest.EndDateTo,
            filterRequest.Spot is not null ? (Spot)(int)filterRequest.Spot : null);

        var result = await sender.Send(query, ct);

        return result.Match(
            Results.Ok,
            error => error.ToProblem());
    }
}

public record WorkOrderFilterRequest(
    [FromQuery] string? SearchTerm,
    [FromQuery] string? SortColumn,
    [FromQuery] string? SortDirection,
    [FromQuery] int? State,
    [FromQuery] Guid? VehicleId,
    [FromQuery] Guid? LaborId,
    [FromQuery] DateTime? StartDateFrom,
    [FromQuery] DateTime? StartDateTo,
    [FromQuery] DateTime? EndDateFrom,
    [FromQuery] DateTime? EndDateTo,
    [FromQuery] int? Spot);

public record PageRequest(
    [FromQuery] int Page = 1,
    [FromQuery] int PageSize = 10);