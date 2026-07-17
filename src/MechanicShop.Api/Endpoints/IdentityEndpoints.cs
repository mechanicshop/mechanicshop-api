using Asp.Versioning.Builder;

using MechanicShop.Api.Extensions;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Application.Features.Identity.Dtos;
using MechanicShop.Application.Features.Identity.Queries.GenerateTokens;
using MechanicShop.Application.Features.Identity.Queries.GetUserInfo;
using MechanicShop.Application.Features.Identity.Queries.RefreshTokens;

using MediatR;

namespace MechanicShop.Api.Endpoints;

public static class IdentityEndpoints
{
    public static IEndpointRouteBuilder MapIdentityEndpoints(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        var group = app.MapGroup("/api/v{version:apiVersion}/identity")
            .WithApiVersionSet(versionSet)
            .HasApiVersion(1.0)
            .MapToApiVersion(1.0);

        group.MapPost("/token/generate",
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
            .WithName("GenerateToken");

        group.MapPost("/token/refresh-token", async (RefreshTokenRequest request, HttpContext context, ISender sender, CancellationToken ct) =>
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
            .WithName("RefreshToken");

        group.MapGet("/current-user/claims", async (ISender sender, IUser user, CancellationToken ct) =>
            {
                var result = await sender.Send(new GetUserByIdQuery(user.Id), ct);

                return result.Match(Results.Ok, error => error.ToProblem());
            })
            .RequireAuthorization()
            .WithName("GetCurrentUserClaims");

        return app;
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
