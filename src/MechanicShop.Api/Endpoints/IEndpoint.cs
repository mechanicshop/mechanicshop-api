using Asp.Versioning.Builder;

namespace MechanicShop.Api.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet);
}
