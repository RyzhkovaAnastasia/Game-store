using MongoDB.Driver;

namespace OnlineGameStore.DAL.Interfaces
{
    public interface INorthwindDBContext
    {
        IMongoCollection<TEntity> GetCollection<TEntity>(string collectionName);
    }
}