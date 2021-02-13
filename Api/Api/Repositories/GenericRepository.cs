using Api.Repositories.Configuration;
using Api.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, IEntity, new()
    {
        Task<TEntity> Add(TEntity entity);
        Task<List<TEntity>> AddRange(List<TEntity> entity);
        Task<TEntity> Update(TEntity entity);
        Task Delete(Guid entityId);
        Task<List<TEntity>> ReadAll();
    }

    public class GenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        protected readonly IGenericDbContext<TEntity> dbContext;

        public GenericRepository(IGenericDbContext<TEntity> dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            try
            {
                dbContext.Add(entity);
                await dbContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't add entity: {ex.Message}");
            }
        }

        public async Task<List<TEntity>> AddRange(List<TEntity> entities)
        {
            try
            {
                dbContext.AddRange(entities);
                await dbContext.SaveChangesAsync();
                return entities;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't add entities: {ex.Message}");
            }
        }

        public async Task Delete(Guid entityId)
        {
            try
            {
                var entity = dbContext.ReadById(entityId);
                dbContext.Delete(entity);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't remove entity: {ex.Message}");
            }
        }

        public async Task<List<TEntity>> ReadAll()
        {
            try
            {
                 var entities = await dbContext.GetQueryable().ToListAsync();
                return entities;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't read all entities: {ex.Message}");
            }
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            try
            {
                dbContext.Update(entity);
                await dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't read all entities: {ex.Message}");
            }
        }
    }
}
