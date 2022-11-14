using Microsoft.EntityFrameworkCore;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineGameStore.DAL.Repositories
{
    public class SQLRepository<TEntity> : ISQLRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly IGameStoreDBContext _gameStoreDBContext;
        public SQLRepository(IGameStoreDBContext gameStoreDBContext)
        {
            _gameStoreDBContext = gameStoreDBContext;
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            if (entity != null)
            {
                entity.Created = DateTime.UtcNow;
                await _gameStoreDBContext.Set<TEntity>().AddAsync(entity);
                await _gameStoreDBContext.SaveChangesAsync(true);
            }
            return entity;
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            TEntity entityToDelete = await GetByIdAsync(id);
            if (entityToDelete != null)
            {
                _gameStoreDBContext.Set<TEntity>().Remove(entityToDelete);
                await _gameStoreDBContext.SaveChangesAsync(true);
            }
        }

        public virtual async Task<TEntity> EditAsync(TEntity entity)
        {
            if (entity != null)
            {
                var oldEntity = await _gameStoreDBContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.Id);

                if (oldEntity != null)
                {
                    entity.Updated = DateTime.UtcNow;
                    _gameStoreDBContext.Set<TEntity>().Update(entity);
                    await _gameStoreDBContext.SaveChangesAsync(true);

                    return entity;
                }
                return oldEntity;
            }
            return entity;
        }

        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
            {
                return null;
            }
            return await _gameStoreDBContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _gameStoreDBContext.Set<TEntity>().ToListAsync();
            }
            return await _gameStoreDBContext.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetWhereAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _gameStoreDBContext.Set<TEntity>().AsNoTracking().Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _gameStoreDBContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> EditFieldAsync(TEntity entity, string field)
        {
            var entityToUpdate = await GetByIdAsync(entity.Id);

            if (entityToUpdate != null)
            {
                if (entity.GetType().GetProperties().Any(p => p.Name == field))
                {
                    var prop = entity.GetType().GetProperty(field);
                    var value = prop.GetValue(entity);

                    prop.SetValue(entityToUpdate, value);

                    _gameStoreDBContext.Set<TEntity>().Update(entityToUpdate);
                    await _gameStoreDBContext.SaveChangesAsync(true);
                }
            }
            return entityToUpdate;
        }
    }
}