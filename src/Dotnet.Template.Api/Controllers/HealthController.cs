namespace Dotnet.Template.Api.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("/healthz")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}
