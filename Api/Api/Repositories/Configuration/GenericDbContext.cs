using Api.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Api.Repositories.Configuration
{
    public interface IGenericDbContext<TEntity> where TEntity : IEntity, new() {

        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);
        void Delete(TEntity entity);
        TEntity ReadById(Guid id);
        IQueryable<TEntity> ReadAll();
        Task SaveChangesAsync();
    }

    public class GenericDbContext<TEntity> : DbContext, IGenericDbContext<TEntity>
        where TEntity : class, IEntity, new()
    {
        private DbSet<TEntity> Entities { get; set; }

        public GenericDbContext(DbContextOptions<GenericDbContext<TEntity>> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public TEntity Add(TEntity entity)
        {
            Entities.Add(entity);

            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            return entity;
        }

        public void Delete(TEntity entity)
        {
            Entities.Remove(entity);
        }

        public TEntity ReadById(Guid id)
        {
            return Entities.Where(e => e.Id == id).FirstOrDefault();
        }

        public IQueryable<TEntity> ReadAll()
        {
            return Entities;
        }

        Task IGenericDbContext<TEntity>.SaveChangesAsync()
        {
            return SaveChangesAsync();
        }
    }
}
