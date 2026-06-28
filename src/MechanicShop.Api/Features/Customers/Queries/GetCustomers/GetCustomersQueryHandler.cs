using MechanicShop.Api.Common.Interfaces;
using MechanicShop.Api.Domain.Common.Results;
using MechanicShop.Api.Features.Customers.Dtos;
using MechanicShop.Api.Features.Customers.Mappers;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace MechanicShop.Api.Features.Customers.Queries.GetCustomers;

public class GetCustomersQueryHandler(IAppDbContext context)
    : IRequestHandler<GetCustomersQuery, Result<List<CustomerDto>>>
{
    public async Task<Result<List<CustomerDto>>> Handle(GetCustomersQuery query, CancellationToken ct)
    {
        var customers = await context.Customers.Include(c => c.Vehicles).AsNoTracking().ToListAsync(ct);

        return customers.ToDtos();
    }
}