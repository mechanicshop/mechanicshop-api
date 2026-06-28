using MechanicShop.Api.Domain.Common.Results;

using MediatR;

namespace MechanicShop.Api.Features.Customers.Commands.RemoveCustomer;

public sealed record RemoveCustomerCommand(Guid CustomerId)
    : IRequest<Result<Deleted>>;