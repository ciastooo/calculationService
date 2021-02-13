using Api.Services.RabbitMq;
using Api.Services.RabbitMq.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> logger;
        protected readonly IRabbitMqRpcService rabbitMqRpcService;

        public HealthController(ILogger<HealthController> logger, IRabbitMqRpcService rabbitMqRpcService)
        {
            this.logger = logger;
            this.rabbitMqRpcService = rabbitMqRpcService;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await rabbitMqRpcService.Send<string>(MessageType.Ping, null);
            }
            catch(Exception)
            {
                return NotFound();
            }
            
            logger.LogInformation("This is just a healthcheck - have a nice day :)");

            return Ok();
        }
    }
}
