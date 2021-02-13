using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> logger;


        public HealthController(ILogger<HealthController> logger)
        {
            this.logger = logger;
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
