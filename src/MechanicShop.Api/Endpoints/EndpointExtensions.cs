using Asp.Versioning;

namespace MechanicShop.Api.Endpoints;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapAllEndpoints(this IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        return app
            .MapCustomerEndpoints(versionSet)
            .MapWorkOrderEndpoints(versionSet)
            .MapBillingEndpoints(versionSet)
            .MapRepairTaskEndpoints(versionSet)
            .MapDashboardEndpoints(versionSet)
            .MapLaborEndpoints(versionSet)
            .MapSchedulingEndpoints(versionSet)
            .MapIdentityEndpoints(versionSet)
            .MapSettingsEndpoints(versionSet);
    }
}
