using Asp.Versioning.Builder;

using MechanicShop.Api.Common.Interfaces;
using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;
using MechanicShop.Api.Features.Identity.Dtos;
using MechanicShop.Api.Features.Identity.Queries.GetUserInfo;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.Identity.Queries.GetUserInfo;

public class GetUserInfoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        app.MapGet("/identity/current-user/claims", async (ISender sender, IUser user, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetUserByIdQuery(user.Id), ct);
            return result.Match(Results.Ok, error => error.ToProblem());
        })
        .RequireAuthorization()
        .WithName("GetCurrentUserClaims")
        .WithSummary("Gets current authenticated user info.")
        .WithDescription("Returns user details extracted from the current JWT access token.")
        .Produces<AppUserDto>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}