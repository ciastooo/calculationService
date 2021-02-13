using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Controllers.Dtos;
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
        protected readonly ITimeseriesRepository repository;

        public TimeseriesController(ILogger<TimeseriesController> logger, ITimeseriesRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> AddData([FromBody]List<TimeseriesDto> data)
        {
            var entitiesToAdd = data.Select(x => new Timeseries
            {
                Id = Guid.NewGuid(),
                Name = x.Name,
                Timestamp = x.T,
                Value = x.V,
            }).ToList();

            await repository.AddRange(entitiesToAdd);

            return Accepted();
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> QueryData([FromRoute]string name, [FromQuery]DateTime? from, [FromQuery] DateTime? to)
        {
            var data = await repository.Query(name, from, to);

            var sum = data.Sum(x => x.Value);
            var average = data.Average(x => x.Value);

            return Ok(new TimeseriesCalculationResultDto { avg = average, sum = sum });
        }
    }
}
