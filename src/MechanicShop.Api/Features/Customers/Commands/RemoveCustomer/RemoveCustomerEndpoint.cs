using Asp.Versioning.Builder;

using MechanicShop.Api.Domain.Identity;
using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.Customers.Commands.RemoveCustomer;

public class RemoveCustomerEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        app.MapDelete("/api/v{version:apiVersion}/customers/{customerId:guid}", async (Guid customerId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new RemoveCustomerCommand(customerId), ct);
            return result.Match(_ => Results.NoContent(), e => e.ToProblem());
        })
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(1.0)
        .RequireAuthorization(policy => policy.RequireRole(nameof(Role.Manager)))
        .WithName("RemoveCustomer")
        .WithSummary("Removes a customer.")
        .WithDescription("Deletes the specified customer from the system.")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
        .MapToApiVersion(1.0);
    }
}
