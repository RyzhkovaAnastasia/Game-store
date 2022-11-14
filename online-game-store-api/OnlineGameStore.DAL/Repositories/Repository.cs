
using MongoDB.Bson;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineGameStore.DAL.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ISQLRepository<TEntity> _sqlRepository;
        private readonly IMongoRepository<TEntity> _mongoRepository;
        private readonly ILogger _logger;

        public Repository(
            ISQLRepository<TEntity> sqlRepo = null,
            IMongoRepository<TEntity> mongoRepo = null,
            ILogger logger = null)
        {
            _sqlRepository = sqlRepo;
            _mongoRepository = mongoRepo;
            _logger = logger;
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            _logger?.Information("{Date} {Action} {Type}", DateTime.UtcNow, "Create", typeof(TEntity));

            return await _sqlRepository?.CreateAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            _logger?.Information("{Date} {Action} {Type}", DateTime.UtcNow, "Delete", typeof(TEntity));

            await _sqlRepository?.DeleteAsync(id);
        }

        public async Task<TEntity> EditAsync(TEntity entity)
        {
            if (entity != null)
            {
                var editResult = await _sqlRepository?.EditAsync(entity);
                if (editResult == null)
                {
                    _logger?.Information("{Date} {Action} {Type}", DateTime.UtcNow, "Create", typeof(TEntity));

                    entity.ObjectId = ObjectId.Empty;
                    return await _sqlRepository?.CreateAsync(entity);
                }

                _logger?.Information("{Date} {Action} {Type}", DateTime.UtcNow, "Edit", typeof(TEntity));
            }
            return entity;
        }

        public async Task<TEntity> EditFieldAsync(TEntity entity, string field)
        {
            if (entity != null && !string.IsNullOrEmpty(field))
            {
                var editedEntity = await _sqlRepository?.EditFieldAsync(entity, field);
                if (editedEntity == null)
                {
                    await _mongoRepository?.EditFieldAsync(entity, field);
                }
                _logger?.Information("{Date} {Action} {Type}", DateTime.UtcNow, "Edit", typeof(TEntity));
            }
            return entity;
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var result = await _sqlRepository?.FindAsync(predicate);

            if (_mongoRepository != null && result == null)
            {
                result = await _mongoRepository?.FindAsync(predicate);
            }
            return result;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (_sqlRepository != null && _mongoRepository == null)
            {
                return await _sqlRepository.GetAllAsync(predicate);
            }

            if (_sqlRepository == null && _mongoRepository != null)
            {
                return await _mongoRepository.GetAllAsync(predicate);
            }

            var sqlEntities = await _sqlRepository.GetAllAsync(x => !x.IsDeleted);
            var mongoEntities = await _mongoRepository.GetAllAsync();

            if (predicate == null)
            {
                return sqlEntities.Concat(mongoEntities).GroupBy(x => x.Id).Select(x => x.First());
            }

            return sqlEntities.Concat(mongoEntities).Where(predicate.Compile());
        }

        public async Task<IEnumerable<TEntity>> GetWhereAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var sqlEntities = await _sqlRepository?.GetWhereAsNoTrackingAsync(predicate);
            if (_mongoRepository != null)
            {
                var mongoEntities = await _mongoRepository?.GetAllAsync(predicate);
                return sqlEntities.Concat(mongoEntities);
            }
            return sqlEntities;
        }

        public async Task SoftDeleteAsync(Guid id)
        {
            TEntity entity = await _sqlRepository?.GetByIdAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                await EditAsync(entity);
            }
            else
            {
                entity = await _mongoRepository?.FindAsync(x => x.Id == id);
                if (entity != null)
                {
                    entity.IsDeleted = true;
                    await _sqlRepository?.CreateAsync(entity);
                }
            }
            _logger?.Information("{Date} {Action} {Type}", DateTime.UtcNow, "Soft delete", typeof(TEntity));

        }
    }
}
