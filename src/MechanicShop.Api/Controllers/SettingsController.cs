using Asp.Versioning;

using MechanicShop.Api.DTOs.Responses;
using MechanicShop.Api.Infrastructure.Settings;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MechanicShop.Api.Controllers;

[Route("api/v{version:apiVersion}/settings")]
[ApiVersion("1.0")]
[Authorize]
public class SettingsController : ApiController
{
    private readonly IOptions<AppSettings> _options;

    public SettingsController(IOptions<AppSettings> options)
    {
        _options = options;
    }

    [HttpGet("operating-hours")]
    public ActionResult GetOperatingHours()
    {
        return Ok(new OperatingHoursResponse(_options.Value.OpeningTime, _options.Value.ClosingTime));
    }
}
