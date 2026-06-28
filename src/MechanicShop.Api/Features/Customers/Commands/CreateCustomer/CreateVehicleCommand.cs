using MechanicShop.Api.Domain.Common.Results;
using MechanicShop.Api.Features.Customers.Dtos;

using MediatR;

namespace MechanicShop.Api.Features.Customers.Commands.CreateCustomer;

public sealed record CreateVehicleCommand(
 string Make,
 string Model,
 int Year,
 string LicensePlate) : IRequest<Result<VehicleDto>>;
