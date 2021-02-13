using Api.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Configuration
{
    public class TimeseriesContext : DbContext
    {
        public DbSet<Timeseries> Timeseries { get; set; }

        public TimeseriesContext(DbContextOptions<TimeseriesContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
