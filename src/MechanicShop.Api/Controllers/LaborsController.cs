using Asp.Versioning;

using MechanicShop.Application.Features.Labors.Queries.GetLabors;
using MechanicShop.Domain.Identity;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Controllers;

[Route("api/v{version:apiVersion}/labors")]
[ApiVersion("1.0")]
[Authorize(Roles = nameof(Role.Manager))]
public class LaborsController(ISender sender) : ApiController
{
    [HttpGet]
    public async Task<ActionResult> GetLabors(CancellationToken ct)
    {
        var result = await sender.Send(new GetLaborsQuery(), ct);

        return result.Match(Ok, Problem);
    }
}
