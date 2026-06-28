using Asp.Versioning.Builder;

using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;
using MechanicShop.Api.Features.Identity.Dtos;
using MechanicShop.Api.Features.Identity.Queries.RefreshTokens;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.Identity.Queries.RefreshTokens;

public class RefreshTokenEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        app.MapPost("/identity/token/refresh-token", async (RefreshTokenQuery request, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(request, ct);
            return result.Match(Results.Ok, error => error.ToProblem());
        })
        .WithName("RefreshToken")
        .WithSummary("Refreshes an active JWT token.")
        .WithDescription("Uses a valid refresh token to obtain a new access token.")
        .Produces<TokenResponse>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}