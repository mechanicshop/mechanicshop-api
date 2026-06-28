using Asp.Versioning.Builder;

using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;
using MechanicShop.Api.Features.Customers.Dtos;
using MechanicShop.Api.Features.Customers.Queries.GetCustomers;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.Customers.Queries.GetCustomers;

public class GetCustomersEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        app.MapGet("/api/v{version:apiVersion}/customers", async (ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetCustomersQuery(), ct);
            return result.Match(Results.Ok, e => e.ToProblem());
        })
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(1.0)
        .RequireAuthorization()
        .WithName("GetCustomers")
        .WithSummary("Retrieves all customers.")
        .WithDescription("Returns a list of all registered customers.")
        .Produces<List<CustomerDto>>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
        .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)))
        .MapToApiVersion(1.0);
    }
}