using MechanicShop.Api.Common.Interfaces;
using MechanicShop.Api.Domain.Common.Results;
using MechanicShop.Api.Domain.Customers;
using MechanicShop.Api.Domain.Customers.Vehicles;
using MechanicShop.Api.Features.Customers.Dtos;
using MechanicShop.Api.Features.Customers.Mappers;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace MechanicShop.Api.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler(
    ILogger<CreateCustomerCommandHandler> logger,
    IAppDbContext context,
    HybridCache cache
    )
    : IRequestHandler<CreateCustomerCommand, Result<CustomerDto>>
{
    public async Task<Result<CustomerDto>> Handle(CreateCustomerCommand command, CancellationToken ct)
    {
        var email = command.Email.Trim().ToLower();

        var exists = await context.Customers.AnyAsync(
            c => c.Email!.ToLower() == email,
            ct);

        if (exists)
        {
            logger.LogWarning("Customer creation aborted. Email already exists.");

            return CustomerErrors.CustomerExists;
        }

        List<Vehicle> vehicles = [];

        foreach (var v in command.Vehicles)
        {
            var vehicleResult = Vehicle.Create(Guid.NewGuid(), v.Make, v.Model, v.Year, v.LicensePlate);

            if (vehicleResult.IsError)
            {
                return vehicleResult.Errors;
            }

            vehicles.Add(vehicleResult.Value);
        }

        var createCustomerResult = Customer.Create(
            Guid.NewGuid(),
            command.Name.Trim(),
            command.PhoneNumber.Trim(),
            command.Email.Trim(),
            vehicles);

        if (createCustomerResult.IsError)
        {
            return createCustomerResult.Errors;
        }

        context.Customers.Add(createCustomerResult.Value);

        await context.SaveChangesAsync(ct);

        await cache.RemoveByTagAsync("customer", ct);

        var customer = createCustomerResult.Value;

        logger.LogInformation("Customer created successfully. Id: {CustomerId}", createCustomerResult.Value.Id);

        return customer.ToDto();
    }
}
