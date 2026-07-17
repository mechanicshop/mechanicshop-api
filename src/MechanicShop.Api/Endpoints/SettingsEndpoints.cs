using Asp.Versioning.Builder;

using MechanicShop.Api.DTOs.Responses;
using MechanicShop.Infrastructure.Settings;

using Microsoft.Extensions.Options;

namespace MechanicShop.Api.Endpoints;

public static class SettingsEndpoints
{
    public static IEndpointRouteBuilder MapSettingsEndpoints(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        var group = app.MapGroup("/api/v{version:apiVersion}/settings")
            .WithApiVersionSet(versionSet)
            .HasApiVersion(1.0)
            .RequireAuthorization()
            .MapToApiVersion(1.0);

        group.MapGet("/operating-hours", (IOptions<AppSettings> options) =>
        {
            return Results.Ok(new OperatingHoursResponse(options.Value.OpeningTime, options.Value.ClosingTime));
        })
        .WithName("GetOperatingHours");

        return app;
    }
}
