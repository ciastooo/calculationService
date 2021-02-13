using System.ComponentModel.DataAnnotations;

namespace Api.Controllers.Dtos
{
    public class TimeseriesDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public long T { get; set; }
        [Required]
        public decimal V{ get; set; }
    }

    public class TimeseriesCalculationResultDto
    {       
        public double avg { get; set; }
        public double sum { get; set; }
    }
}
