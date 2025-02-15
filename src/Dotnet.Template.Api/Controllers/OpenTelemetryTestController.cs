namespace Dotnet.Template.Api.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("api/v1/open-telemetry-test")]
public class OpenTelemetryTestController : ControllerBase
{
    private readonly ILogger<OpenTelemetryTestController> _logger;

    public OpenTelemetryTestController(ILogger<OpenTelemetryTestController> logger)
    {
        this._logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var randomInteger = new Random().Next(0, 10);
        this._logger.LogInformation("Returning {randomInteger} to clientIpAddress {ClientIp}", randomInteger, HttpContext.Connection.RemoteIpAddress.ToString());
        return Ok(randomInteger);
    }
}
