using Api.Repositories.Configuration;
using Api.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Repositories
{
    public interface ITimeseriesRepository : IRepository<Timeseries>
    {
        Task<List<Timeseries>> Query(string name, long? from, long? to);
    }

    public class TimeseriesRepository : GenericRepository<Timeseries>, ITimeseriesRepository
    {
        public TimeseriesRepository(IGenericDbContext<Timeseries> dbContext) : base(dbContext)
        {

        }

        public async Task<List<Timeseries>> Query(string name, long? from, long? to)
        {
            try
            {
                var query = dbContext.GetQueryable().Where(x => x.Name == name);

                if (from.HasValue)
                {
                    var fromDate = DateTimeOffset.FromUnixTimeSeconds(from.Value).DateTime;
                    query = query.Where(x => x.Timestamp >= fromDate);
                }

                if (to.HasValue)
                {
                    var toDate = DateTimeOffset.FromUnixTimeSeconds(to.Value).DateTime;
                    query = query.Where(x => x.Timestamp <= toDate);
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't query timeseries: {ex.Message}");
            }
        }
    }
}
