using MechanicShop.Api.Common.Interfaces;
using MechanicShop.Api.Domain.Common.Results;
using MechanicShop.Api.Domain.Identity;
using MechanicShop.Api.Features.Labors.Dtos;
using MechanicShop.Api.Features.Labors.Mappers;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace MechanicShop.Api.Features.Labors.Queries.GetLabors;

public class GetLaborsQueryHandler(IAppDbContext context)
    : IRequestHandler<GetLaborsQuery, Result<List<LaborDto>>>
{
    public async Task<Result<List<LaborDto>>> Handle(GetLaborsQuery query, CancellationToken ct)
    {
        var labors = await context.Employees.AsNoTracking().Where(e => e.Role == Role.Labor).ToListAsync(ct);

        return labors.ToDtos();
    }
}
