using Asp.Versioning.Builder;

using MechanicShop.Api.DTOs.Responses;
using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Infrastructure.Settings;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MechanicShop.Api.Features.Settings.GetOperatingHours;

public class GetOperatingHoursEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        // Notice we don't need the versionSet as settings is not versioned under /api/v{version}
        app.MapGet("/api/settings/operating-hours", (IOptions<AppSettings> options) =>
        {
            return Results.Ok(new OperatingHoursResponse(options.Value.OpeningTime, options.Value.ClosingTime));
        })
        .RequireAuthorization()
        .WithName("GetOperatingHours")
        .WithSummary("Retrieves the application's operating hours.")
        .WithDescription("Returns the configured opening and closing times for the system.")
        .Produces<OperatingHoursResponse>()
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}