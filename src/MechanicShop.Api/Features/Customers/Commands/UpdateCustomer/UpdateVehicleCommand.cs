using MechanicShop.Api.Domain.Common.Results;

using MediatR;

namespace MechanicShop.Api.Features.Customers.Commands.UpdateCustomer;

public sealed record UpdateVehicleCommand(
 Guid? VehicleId,
 string Make,
 string Model,
 int Year,
 string LicensePlate) : IRequest<Result<Updated>>;
