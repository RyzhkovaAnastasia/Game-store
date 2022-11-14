using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineGameStore.DAL.Interfaces
{
    public interface ISQLRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null);
        Task<TEntity> GetByIdAsync(Guid id);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetWhereAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> EditAsync(TEntity entity);
        Task<TEntity> EditFieldAsync(TEntity entity, string field);
        Task DeleteAsync(Guid id);
    }
}
