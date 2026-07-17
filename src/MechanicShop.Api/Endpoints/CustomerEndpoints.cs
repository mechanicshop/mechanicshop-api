using Asp.Versioning.Builder;

using MechanicShop.Api.DTOs.Requests.Customers;
using MechanicShop.Api.Extensions;
using MechanicShop.Application.Features.Customers.Commands.CreateCustomer;
using MechanicShop.Application.Features.Customers.Commands.RemoveCustomer;
using MechanicShop.Application.Features.Customers.Commands.UpdateCustomer;
using MechanicShop.Application.Features.Customers.Queries.GetCustomerById;
using MechanicShop.Application.Features.Customers.Queries.GetCustomers;
using MechanicShop.Domain.Identity;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Endpoints;

public static class CustomerEndpoints
{
    public static IEndpointRouteBuilder MapCustomerEndpoints(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        var group = app.MapGroup("/api/v{version:apiVersion}/customers")
            .WithApiVersionSet(versionSet)
            .HasApiVersion(1.0)
            .RequireAuthorization()
            .MapToApiVersion(1.0);

        group.MapGet("/", async (ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetCustomersQuery(), ct);

            return result.Match(Results.Ok, e => e.ToProblem());
        })
        .WithName("GetCustomers")
        .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)));

        group.MapGet("/{customerId:guid}", async (Guid customerId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetCustomerByIdQuery(customerId), ct);

            return result.Match(Results.Ok, e => e.ToProblem());
        })
        .WithName("GetCustomerById")
        .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)));

        group.MapPost("/", async ([FromBody] CreateCustomerRequest request, ISender sender, CancellationToken ct) =>
        {
            var vehicles = request.Vehicles
                .ConvertAll(v => new CreateVehicleCommand(v.Make, v.Model, v.Year, v.LicensePlate));

            var result = await sender.Send(
                new CreateCustomerCommand(request.Name, request.PhoneNumber, request.Email, vehicles), ct);

            return result.Match(
                r => Results.CreatedAtRoute("GetCustomerById", new { version = "1.0", customerId = r.CustomerId }, r),
                e => e.ToProblem());
        })
        .RequireAuthorization("ManagerOnly")
        .WithName("CreateCustomer");

        group.MapPut("/{customerId:guid}", async (Guid customerId, [FromBody] UpdateCustomerRequest request, ISender sender, CancellationToken ct) =>
        {
            var vehicles = request.Vehicles
                .ConvertAll(v => new UpdateVehicleCommand(v.VehicleId, v.Make, v.Model, v.Year, v.LicensePlate));

            var result = await sender.Send(
                new UpdateCustomerCommand(customerId, request.Name, request.PhoneNumber, request.Email, vehicles), ct);

            return result.Match(_ => Results.Created(), e => e.ToProblem());
        })
        .RequireAuthorization(policy => policy.RequireRole(nameof(Role.Manager)))
        .WithName("UpdateCustomer");

        group.MapDelete("/{customerId:guid}", async (Guid customerId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new RemoveCustomerCommand(customerId), ct);

            return result.Match(_ => Results.NoContent(), e => e.ToProblem());
        })
        .RequireAuthorization(policy => policy.RequireRole(nameof(Role.Manager)))
        .WithName("RemoveCustomer");

        return app;
    }
}
