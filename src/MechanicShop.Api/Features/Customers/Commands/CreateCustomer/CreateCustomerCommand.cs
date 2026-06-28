using MechanicShop.Api.Domain.Common.Results;
using MechanicShop.Api.Features.Customers.Dtos;

using MediatR;

namespace MechanicShop.Api.Features.Customers.Commands.CreateCustomer;

public sealed record CreateCustomerCommand(
    string Name,
    string PhoneNumber,
    string Email,
    List<CreateVehicleCommand> Vehicles

) : IRequest<Result<CustomerDto>>;
