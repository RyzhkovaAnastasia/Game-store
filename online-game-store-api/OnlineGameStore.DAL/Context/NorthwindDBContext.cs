using MongoDB.Driver;
using OnlineGameStore.DAL.Interfaces;

namespace OnlineGameStore.DAL.Context
{
    public class NorthwindDBContext : INorthwindDBContext
    {
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _mongoDB;
        public NorthwindDBContext(string connection, string dbName)
        {
            _mongoClient = new MongoClient(connection);
            _mongoDB = _mongoClient.GetDatabase(dbName);
        }

        public NorthwindDBContext(IMongoClient mongoClient, string dbName)
        {
            _mongoClient = mongoClient;
            _mongoDB = _mongoClient.GetDatabase(dbName);
        }

        public IMongoCollection<TEntity> GetCollection<TEntity>(string collectionName)
        {
            if (string.IsNullOrEmpty(collectionName))
            {
                return null;
            }
            return _mongoDB.GetCollection<TEntity>(collectionName);
        }
    }
}
