using System.Security.Claims;

using MechanicShop.Api.Domain.Common.Results;
using MechanicShop.Api.Features.Identity;
using MechanicShop.Api.Features.Identity.Dtos;

namespace MechanicShop.Api.Common.Interfaces;

public interface ITokenProvider
{
    Task<Result<TokenResponse>> GenerateJwtTokenAsync(AppUserDto user, CancellationToken ct = default);

    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
