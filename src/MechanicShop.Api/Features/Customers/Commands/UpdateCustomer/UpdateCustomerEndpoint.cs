using Asp.Versioning.Builder;

using MechanicShop.Api.Domain.Identity;
using MechanicShop.Api.DTOs.Requests.Customers;
using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;
using MechanicShop.Api.Features.Customers.Commands.UpdateCustomer;
using MechanicShop.Api.Features.Customers.Dtos;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        app.MapPut("/api/v{version:apiVersion}/customers/{customerId:guid}", async (Guid customerId, [FromBody] UpdateCustomerRequest request, ISender sender, CancellationToken ct) =>
        {
            var vehicles = request.Vehicles
                .ConvertAll(v => new UpdateVehicleCommand(v.VehicleId, v.Make, v.Model, v.Year, v.LicensePlate));

            var result = await sender.Send(
                new UpdateCustomerCommand(
                    customerId,
                    request.Name,
                    request.PhoneNumber,
                    request.Email,
                    vehicles),
                ct);

            return result.Match(_ => Results.Created(), e => e.ToProblem());
        })
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(1.0)
        .RequireAuthorization(policy => policy.RequireRole(nameof(Role.Manager)))
        .WithName("UpdateCustomer")
        .WithSummary("Updates an existing customer.")
        .WithDescription("Updates a customer and its associated vehicle.")
        .Produces<CustomerDto>(StatusCodes.Status200OK)
        .Produces<ValidationProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
        .MapToApiVersion(1.0);
    }
}