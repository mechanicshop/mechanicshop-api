using System.Security.Claims;

using MechanicShop.Api.Common.Interfaces;
using MechanicShop.Api.Domain.Identity;

using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace MechanicShop.Api.Infrastructure.Identity.Policies;

public class LaborAssignedRequirement : IAuthorizationRequirement;

public class LaborAssignedHandler(IAppDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
    : AuthorizationHandler<LaborAssignedRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        LaborAssignedRequirement requirement)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            context.Fail();
            return;
        }

        // Extract WorkOrderId dynamically from the route
        var workOrderIdString = httpContextAccessor.HttpContext?.Request.RouteValues["WorkOrderId"]?.ToString();

        if (!Guid.TryParse(workOrderIdString, out var workOrderId))
        {
            context.Fail();
            return;
        }

        var isAssigned = await appDbContext.WorkOrders
            .AnyAsync(a => a.Id == workOrderId && a.LaborId == Guid.Parse(userId));

        if (isAssigned)
        {
            context.Succeed(requirement);
            return;
        }

        if (context.User.IsInRole(nameof(Role.Manager)))
        {
            context.Succeed(requirement);
            return;
        }

        context.Fail();
    }
}
