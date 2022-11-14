using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineGameStore.DAL.Interfaces
{
    public interface IMongoRepository<TEntity> where TEntity : class
    {
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null);
        Task<TEntity> EditFieldAsync(TEntity entity, string field);
    }
}
