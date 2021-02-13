using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Repositories.Configuration;
using Api.Repositories.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimeseriesController : ControllerBase
    {
        private readonly ILogger<TimeseriesController> _logger;
        protected readonly TimeseriesContext dbContext;

        public TimeseriesController(ILogger<TimeseriesController> logger, TimeseriesContext dbContext)
        {
            _logger = logger;
            this.dbContext = dbContext;
        }

        [HttpGet("test")]
        public async Task<IEnumerable<Timeseries>> Get()
        {
            var a = dbContext.Timeseries.ToList();
            dbContext.Timeseries.Add(new Timeseries
            {
                Id = Guid.NewGuid(),
                Name = "test",
                Timestamp = DateTime.Now,
                Value = 42
            });
            await dbContext.SaveChangesAsync();
            return a;
        }
    }
}
