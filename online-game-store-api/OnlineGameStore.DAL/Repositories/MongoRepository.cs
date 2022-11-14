using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using OnlineGameStore.Common.Converters;
using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace OnlineGameStore.DAL.Repositories
{
    public class MongoRepository<TEntity> : IMongoRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly IMongoCollection<TEntity> _collection;

        public MongoRepository(INorthwindDBContext context, string collectionName)
        {
            _collection = context.GetCollection<TEntity>(collectionName);
        }

        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
            {
                return null;
            }

            return await _collection.Find(predicate).FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _collection.Find(_ => true).ToListAsync();
            }
            return await _collection.Find(predicate).ToListAsync();
        }

        public async Task<TEntity> EditFieldAsync(TEntity entity, string field)
        {
            if (entity.GetType().GetProperties().Any(p => p.Name == field))
            {
                object propValue = entity.GetType().GetProperty(field).GetValue(entity);
                var updateDefinition = Builders<TEntity>.Update.Set(field, propValue);

                string entityType = typeof(TEntity).GetProperty(nameof(entity.Id)).GetCustomAttribute<BsonElementAttribute>().ElementName;

                var filter = Builders<TEntity>.Filter.AnyEq(entityType, GuidIntConverter.GuidToInt(entity.Id));

                await _collection?.UpdateOneAsync(filter, updateDefinition);
            }
            return entity;
        }
    }
}
