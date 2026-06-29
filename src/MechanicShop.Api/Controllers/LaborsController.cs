using Asp.Versioning;

using MechanicShop.Api.Domain.Identity;
using MechanicShop.Api.Features.Labors.Queries.GetLabors;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Controllers;

[Route("api/v{version:apiVersion}/labors")]
[ApiVersion("1.0")]
[Authorize(Roles = nameof(Role.Manager))]
public class LaborsController : ApiController
{
    private readonly ISender _sender;

    public LaborsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult> GetLabors(CancellationToken ct)
    {
        var result = await _sender.Send(new GetLaborsQuery(), ct);

        return result.Match<ActionResult>(Ok, Problem);
    }
}
