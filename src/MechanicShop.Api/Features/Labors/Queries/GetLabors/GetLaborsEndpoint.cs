using Asp.Versioning.Builder;

using MechanicShop.Api.Domain.Identity;
using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;
using MechanicShop.Api.Features.Labors.Dtos;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.Labors.Queries.GetLabors;

public class GetLaborsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        app.MapGet("/api/v{version:apiVersion}/labors", async (ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetLaborsQuery(), ct);
            return result.Match(Results.Ok, e => e.ToProblem());
        })
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(1.0)
        .RequireAuthorization(policy => policy.RequireRole(nameof(Role.Manager)))
        .WithName("GetLabors")
        .WithSummary("Retrieves all labor definitions.")
        .WithDescription("Returns a list of available labor types that can be assigned to work orders.")
        .Produces<List<LaborDto>>()
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
        .MapToApiVersion(1.0);
    }
}
