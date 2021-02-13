using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Controllers.Dtos
{
    public class TimeseriesDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime T { get; set; }
        [Required]
        public decimal V{ get; set; }
    }

    public class TimeseriesCalculationResultDto
    {       
        public decimal avg { get; set; }
        public decimal sum { get; set; }
    }
}
