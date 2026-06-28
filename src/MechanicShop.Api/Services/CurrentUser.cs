using System.Security.Claims;

using MechanicShop.Api.Common.Interfaces;

namespace MechanicShop.Api.Services;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : IUser
{
    public string? Id => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
}
