using System;

namespace Api.Repositories.Models
{
    public class Timeseries
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Timestamp{ get; set; }
        public decimal Value { get; set; }
    }
}
