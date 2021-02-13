using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Repositories;
using Api.Repositories.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimeseriesController : ControllerBase
    {
        private readonly ILogger<TimeseriesController> logger;
        protected readonly IRepository<Timeseries> repository;

        public TimeseriesController(ILogger<TimeseriesController> logger, IRepository<Timeseries> repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        [HttpGet("test")]
        public async Task<IEnumerable<Timeseries>> Get()
        {
            var a = await repository.ReadAll();
            await repository.Add(new Timeseries
            {
                Id = Guid.NewGuid(),
                Name = "test",
                Timestamp = DateTime.Now,
                Value = 42
            });
            return a;
        }
    }
}
