using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Application.Features.Identity;
using MechanicShop.Application.Features.Identity.Dtos;
using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.Identity;
using MechanicShop.Infrastructure.Settings;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MechanicShop.Infrastructure.Identity;

public class TokenProvider(
    IOptions<JwtSettings> jwtOptions,
    IAppDbContext context,
    IHttpContextAccessor httpContextAccessor) : ITokenProvider
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    public async Task<Result<TokenResponse>> GenerateJwtTokenAsync(
        AppUserDto user,
        CancellationToken ct = default)
    {
        var tokenResult = await CreateAsync(user, ct);

        if (tokenResult.IsError)
        {
            return tokenResult.Errors;
        }

        return tokenResult.Value;
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            ValidateIssuer = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudiences = _jwtSettings.Audiences,
            ValidateLifetime = false, // Ignore token expiration
            ClockSkew = TimeSpan.Zero,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token.");
        }

        return principal;
    }

    private async Task<Result<TokenResponse>> CreateAsync(
        AppUserDto user,
        CancellationToken ct = default)
    {
        var issuer = _jwtSettings.Issuer;
        var audience = GetValidAudienceFromRequest();
        var key = _jwtSettings.Secret;
        var expiryMinutes = _jwtSettings.ExpiryMinutes > 0 ? _jwtSettings.ExpiryMinutes : 60;

        var expires = DateTime.UtcNow.AddMinutes(expiryMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserId), new(JwtRegisteredClaimNames.Email, user.Email),
        };

        foreach (var role in user.Roles)
        {
            claims.Add(new(ClaimTypes.Role, role));
        }

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256Signature),
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateToken(descriptor);

        await context.RefreshTokens
            .Where(rt => rt.UserId == user.UserId)
            .ExecuteDeleteAsync(ct);

        var refreshTokenResult = RefreshToken.Create(
            Guid.NewGuid(),
            GenerateRefreshToken(),
            user.UserId,
            DateTime.UtcNow.AddDays(7));

        if (refreshTokenResult.IsError)
        {
            return refreshTokenResult.Errors;
        }

        var refreshToken = refreshTokenResult.Value;

        context.RefreshTokens.Add(refreshToken);

        await context.SaveChangesAsync(ct);

        return new TokenResponse
        {
            AccessToken = tokenHandler.WriteToken(securityToken),
            RefreshToken = refreshToken.Token,
            ExpiresOnUtc = expires,
        };
    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }

    private string GetValidAudienceFromRequest()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            var origin = httpContext.Request.Headers.Origin.ToString();
            if (!string.IsNullOrWhiteSpace(origin) && _jwtSettings.Audiences.Contains(origin))
            {
                return origin;
            }

            var referer = httpContext.Request.Headers.Referer.ToString();
            if (!string.IsNullOrWhiteSpace(referer))
            {
                foreach (var aud in _jwtSettings.Audiences)
                {
                    if (referer.StartsWith(aud, StringComparison.OrdinalIgnoreCase))
                    {
                        return aud;
                    }
                }
            }
        }

        return _jwtSettings.Audiences.Length > 0 ? _jwtSettings.Audiences[0] : string.Empty;
    }
}
