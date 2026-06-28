using Asp.Versioning.Builder;

using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.Identity.Queries.GenerateTokens;

public class GenerateTokenEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        // Notice this doesn't use the apiVersionSet on MapGroup because it has its own path "/identity/token/generate"
        app.MapPost("/identity/token/generate", async (GenerateTokenQuery request, ISender sender, CancellationToken ct) =>
        {
            ArgumentNullException.ThrowIfNull(sender);
            ArgumentNullException.ThrowIfNull(request);

            var result = await sender.Send(request, ct);
            return result.Match(Results.Ok, error => error.ToProblem());
        })
        .WithName("GenerateToken")
        .WithSummary("Generates a new JWT access and refresh token.")
        .WithDescription("Authenticates a user with credentials and returns a token pair.")
        .Produces<TokenResponse>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}
