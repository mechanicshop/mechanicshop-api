using Asp.Versioning.Builder;

using MechanicShop.Api.Extensions;
using MechanicShop.Application.Features.Labors.Queries.GetLabors;
using MechanicShop.Domain.Identity;

using MediatR;

namespace MechanicShop.Api.Endpoints;

public static class LaborEndpoints
{
    public static IEndpointRouteBuilder MapLaborEndpoints(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        var group = app.MapGroup("/api/v{version:apiVersion}/labors")
            .WithApiVersionSet(versionSet)
            .HasApiVersion(1.0)
            .RequireAuthorization(policy => policy.RequireRole(nameof(Role.Manager)))
            .MapToApiVersion(1.0);

        group.MapGet("/", async (ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetLaborsQuery(), ct);

            return result.Match(Results.Ok, e => e.ToProblem());
        })
        .WithName("GetLabors");

        return app;
    }
}
