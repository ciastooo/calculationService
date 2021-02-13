using System;

namespace Api.Repositories.Models
{
    public class Timeseries : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Timestamp{ get; set; }
        public double Value { get; set; }
    }
}
