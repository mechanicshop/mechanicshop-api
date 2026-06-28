using MechanicShop.Api.Common.Interfaces;
using MechanicShop.Api.Domain.Common.Results;
using MechanicShop.Api.Features.Customers.Dtos;

namespace MechanicShop.Api.Features.Customers.Queries.GetCustomers;

public sealed record GetCustomersQuery : ICachedQuery<Result<List<CustomerDto>>>
{
    public string CacheKey => "customers";
    public string[] Tags => ["customer"];

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);
}
