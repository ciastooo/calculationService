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
        Task<List<Timeseries>> Query(string name, DateTime? from, DateTime? to);
    }

    public class TimeseriesRepository : GenericRepository<Timeseries>, ITimeseriesRepository
    {
        public TimeseriesRepository(IGenericDbContext<Timeseries> dbContext) : base(dbContext)
        {

        }

        public async Task<List<Timeseries>> Query(string name, DateTime? from, DateTime? to)
        {
            try
            {
                var query = dbContext.GetQueryable().Where(x => x.Name == name);

                if (from.HasValue)
                {
                    query = query.Where(x => x.Timestamp >= from.Value);
                }

                if (to.HasValue)
                {
                    query = query.Where(x => x.Timestamp <= to.Value);
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
