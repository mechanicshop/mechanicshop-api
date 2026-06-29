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
        app.MapPost("/identity/token/generate",
                async (GenerateTokenQuery request, HttpContext context, ISender sender, CancellationToken ct) =>
                {
                    var result = await sender.Send(request, ct);
                    return result.Match(
                        tokenResponse =>
                        {
                            if (!string.IsNullOrEmpty(tokenResponse.RefreshToken))
                            {
                                AppendRefreshTokenCookie(context.Response, tokenResponse.RefreshToken,
                                    context.Request.IsHttps);
                            }

                            return Results.Ok(tokenResponse);
                        },
                        error => error.ToProblem());
                })
            .WithName("GenerateToken")
            .WithSummary("Generates a new JWT access and refresh token.")
            .WithDescription("Authenticates a user with credentials and returns a token pair.")
            .Produces<TokenResponse>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }

    private static void AppendRefreshTokenCookie(HttpResponse response, string refreshToken, bool isHttps)
    {
        response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = isHttps,
            SameSite = isHttps ? SameSiteMode.None : SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            Path = "/",
            MaxAge = TimeSpan.FromDays(7),
        });
    }
}

