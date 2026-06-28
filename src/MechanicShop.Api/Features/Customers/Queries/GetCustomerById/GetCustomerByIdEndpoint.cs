using Asp.Versioning.Builder;

using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;
using MechanicShop.Api.Features.Customers.Dtos;
using MechanicShop.Api.Features.Customers.Queries.GetCustomerById;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.Customers.Queries.GetCustomerById;

public class GetCustomerByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        app.MapGet("/api/v{version:apiVersion}/customers/{customerId:guid}", async (Guid customerId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetCustomerByIdQuery(customerId), ct);
            return result.Match(Results.Ok, e => e.ToProblem());
        })
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(1.0)
        .RequireAuthorization()
        .WithName("GetCustomerById")
        .WithSummary("Retrieves a customer by ID.")
        .WithDescription("Returns detailed customer information for the given customer ID.")
        .Produces<CustomerDto>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
        .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)))
        .MapToApiVersion(1.0);
    }
}