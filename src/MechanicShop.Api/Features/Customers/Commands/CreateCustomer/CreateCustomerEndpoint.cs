using Asp.Versioning.Builder;

using MechanicShop.Api.DTOs.Requests.Customers;
using MechanicShop.Api.Features.Customers.Dtos;
using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        app.MapPost("/api/v{version:apiVersion}/customers",
                async ([FromBody] CreateCustomerRequest request, ISender sender, CancellationToken ct) =>
                {
                    var vehicles = request.Vehicles
                        .ConvertAll(v => new CreateVehicleCommand(v.Make, v.Model, v.Year, v.LicensePlate));

                    var result = await sender.Send(
                        new CreateCustomerCommand(
                            request.Name,
                            request.PhoneNumber,
                            request.Email, vehicles),
                        ct);

                    return result.Match(
                        r => Results.CreatedAtRoute("GetCustomerById",
                            new { version = "1.0", customerId = r.CustomerId }, r),
                        e => e.ToProblem());
                })
            .WithApiVersionSet(apiVersionSet)
            .HasApiVersion(1.0)
            .RequireAuthorization("ManagerOnly")
            .WithName("CreateCustomer")
            .WithSummary("Creates a new customer.")
            .WithDescription("Adds a new customer to the system.")
            .Produces<CustomerDto>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .MapToApiVersion(1.0);
    }
}
