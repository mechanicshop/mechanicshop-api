using Asp.Versioning;

using MechanicShop.Api.Common.Interfaces;
using MechanicShop.Api.Features.Identity.Dtos;
using MechanicShop.Api.Features.Identity.Queries.GenerateTokens;
using MechanicShop.Api.Features.Identity.Queries.GetUserInfo;
using MechanicShop.Api.Features.Identity.Queries.RefreshTokens;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Controllers;

[Route("api/v{version:apiVersion}/identity")]
[ApiVersion("1.0")]
public class IdentityController : ApiController
{
    private readonly ISender _sender;

    public IdentityController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("token/generate")]
    public async Task<ActionResult> GenerateToken(GenerateTokenQuery request, CancellationToken ct)
    {
        var httpContext = HttpContext;

        var result = await _sender.Send(request, ct);

        return result.Match<ActionResult>(
            tokenResponse =>
            {
                if (!string.IsNullOrEmpty(tokenResponse.RefreshToken))
                {
                    AppendRefreshTokenCookie(httpContext.Response, tokenResponse.RefreshToken, httpContext.Request.IsHttps);
                }

                return Ok(tokenResponse);
            },
            Problem);
    }

    [HttpPost("token/refresh-token")]
    public async Task<ActionResult> RefreshToken(RefreshTokenRequest request, CancellationToken ct)
    {
        var httpContext = HttpContext;

        if (!httpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken) || string.IsNullOrWhiteSpace(refreshToken))
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized, title: "Unauthorized",
                detail: "Refresh token is missing.");
        }

        var result = await _sender.Send(new RefreshTokenQuery(refreshToken, request.ExpiredAccessToken), ct);

        return result.Match<ActionResult>(
            tokenResponse =>
            {
                if (!string.IsNullOrEmpty(tokenResponse.RefreshToken))
                {
                    AppendRefreshTokenCookie(httpContext.Response, tokenResponse.RefreshToken, httpContext.Request.IsHttps);
                }

                return Ok(tokenResponse);
            },
            Problem);
    }

    [HttpGet("current-user/claims")]
    [Authorize]
    public async Task<ActionResult> GetCurrentUserClaims(IUser user, CancellationToken ct)
    {
        var result = await _sender.Send(new GetUserByIdQuery(user.Id), ct);

        return result.Match<ActionResult>(Ok, Problem);
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
