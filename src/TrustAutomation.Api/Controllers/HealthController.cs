using Microsoft.AspNetCore.Mvc;

namespace TrustAutomation.Api.Controllers
{
    [ApiController]
    public sealed class HealthController : ControllerBase
    {
        [HttpGet("/health")]
        public IActionResult Health() => Ok(new { ok = true, utc = DateTime.UtcNow });
    }
}
