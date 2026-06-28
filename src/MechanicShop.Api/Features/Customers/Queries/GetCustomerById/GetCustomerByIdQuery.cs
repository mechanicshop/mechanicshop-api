using MechanicShop.Api.Common.Interfaces;
using MechanicShop.Api.Domain.Common.Results;
using MechanicShop.Api.Features.Customers.Dtos;

namespace MechanicShop.Api.Features.Customers.Queries.GetCustomerById;

public sealed record GetCustomerByIdQuery(Guid CustomerId) : ICachedQuery<Result<CustomerDto>>
{
    public string CacheKey => $"customer_{CustomerId}";

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);

    public string[] Tags => ["customer"];
}
