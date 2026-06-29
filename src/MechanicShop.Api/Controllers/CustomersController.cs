using Asp.Versioning;

using MechanicShop.Api.Domain.Identity;
using MechanicShop.Api.DTOs.Requests.Customers;
using MechanicShop.Api.Features.Customers.Commands.CreateCustomer;
using MechanicShop.Api.Features.Customers.Commands.RemoveCustomer;
using MechanicShop.Api.Features.Customers.Commands.UpdateCustomer;
using MechanicShop.Api.Features.Customers.Queries.GetCustomerById;
using MechanicShop.Api.Features.Customers.Queries.GetCustomers;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Controllers;

[Route("api/v{version:apiVersion}/customers")]
[ApiVersion("1.0")]
[Authorize]
public class CustomersController : ApiController
{
    private readonly ISender _sender;

    public CustomersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult> GetCustomers(CancellationToken ct)
    {
        var result = await _sender.Send(new GetCustomersQuery(), ct);

        return result.Match<ActionResult>(Ok, Problem);
    }

    [HttpGet("{customerId:guid}")]
    public async Task<ActionResult> GetCustomerById(Guid customerId, CancellationToken ct)
    {
        var result = await _sender.Send(new GetCustomerByIdQuery(customerId), ct);

        return result.Match<ActionResult>(Ok, Problem);
    }

    [HttpPost]
    [Authorize("ManagerOnly")]
    public async Task<ActionResult> CreateCustomer(CreateCustomerRequest request, CancellationToken ct)
    {
        var vehicles = request.Vehicles
            .ConvertAll(v => new CreateVehicleCommand(v.Make, v.Model, v.Year, v.LicensePlate));

        var result = await _sender.Send(
            new CreateCustomerCommand(request.Name, request.PhoneNumber, request.Email, vehicles), ct);

        return result.Match<ActionResult>(
            r => CreatedAtRoute("GetCustomerById", new { version = "1.0", customerId = r.CustomerId }, r),
            Problem);
    }

    [HttpPut("{customerId:guid}")]
    [Authorize(Roles = nameof(Role.Manager))]
    public async Task<ActionResult> UpdateCustomer(Guid customerId, UpdateCustomerRequest request, CancellationToken ct)
    {
        var vehicles = request.Vehicles
            .ConvertAll(v => new UpdateVehicleCommand(v.VehicleId, v.Make, v.Model, v.Year, v.LicensePlate));

        var result = await _sender.Send(
            new UpdateCustomerCommand(customerId, request.Name, request.PhoneNumber, request.Email, vehicles), ct);

        return result.Match<ActionResult>(_ => Created(), Problem);
    }

    [HttpDelete("{customerId:guid}")]
    [Authorize(Roles = nameof(Role.Manager))]
    public async Task<ActionResult> RemoveCustomer(Guid customerId, CancellationToken ct)
    {
        var result = await _sender.Send(new RemoveCustomerCommand(customerId), ct);

        return result.Match<ActionResult>(_ => NoContent(), Problem);
    }
}
