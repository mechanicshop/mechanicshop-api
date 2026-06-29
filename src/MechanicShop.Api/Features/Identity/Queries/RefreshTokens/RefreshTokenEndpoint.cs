using Asp.Versioning.Builder;

using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.Identity.Queries.RefreshTokens;

public class RefreshTokenEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        app.MapPost("/identity/token/refresh-token", async (RefreshTokenRequest request, HttpContext context, ISender sender, CancellationToken ct) =>
        {
            if (!context.Request.Cookies.TryGetValue("refreshToken", out var refreshToken) || string.IsNullOrWhiteSpace(refreshToken))
            {
                return Results.Problem(
                    detail: "Refresh token is missing.",
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: "Unauthorized");
            }

            var result = await sender.Send(new RefreshTokenQuery(refreshToken, request.ExpiredAccessToken), ct);

            return result.Match(
                tokenResponse =>
                {
                    if (!string.IsNullOrEmpty(tokenResponse.RefreshToken))
                    {
                        AppendRefreshTokenCookie(context.Response, tokenResponse.RefreshToken, context.Request.IsHttps);
                    }

                    return Results.Ok(tokenResponse);
                },
                error => error.ToProblem());
        })
        .WithName("RefreshToken")
        .WithSummary("Refreshes an active JWT token.")
        .WithDescription("Uses a valid refresh token to obtain a new access token.")
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

public record RefreshTokenRequest(string ExpiredAccessToken);

