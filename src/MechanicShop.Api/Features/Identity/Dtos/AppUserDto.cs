using System.Security.Claims;

namespace MechanicShop.Api.Features.Identity.Dtos;

public sealed record AppUserDto(string UserId, string Email, IList<string> Roles, IList<Claim> Claims);