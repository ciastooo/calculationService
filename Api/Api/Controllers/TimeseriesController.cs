using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Controllers.Dtos;
using Api.Repositories;
using Api.Repositories.Models;
using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> AddData([FromBody]List<TimeseriesDto> data)
        {
            var entitiesToAdd = data.Select(x => new Timeseries
            {
                Id = Guid.NewGuid(),
                Name = x.Name,
                Timestamp = DateTimeOffset.FromUnixTimeSeconds(x.T).DateTime,
                Value = x.V,
            }).ToList();

            await repository.AddRange(entitiesToAdd);

            return CreatedAtAction(nameof(AddData), new { ids = entitiesToAdd.Select(x => x.Id).ToList() });
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> QueryData([FromRoute]string name, [FromQuery]long? from = null, [FromQuery] long? to = null)
        {
            var data = await repository.Query(name, from, to);

            var sum = data.Any() ? data.Sum(x => x.Value) : 0;
            var average = data.Any() ? data.Average(x => x.Value) : 0;

            return Ok(new TimeseriesCalculationResultDto { avg = average, sum = sum });
        }
    }
}
