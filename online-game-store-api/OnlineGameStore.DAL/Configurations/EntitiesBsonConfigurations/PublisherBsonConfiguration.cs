using MongoDB.Bson.Serialization;
using OnlineGameStore.DAL.BsonSerializers;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.DAL.Configurations.EntitiesBsonConfigurations
{
    internal class PublisherBsonConfiguration
    {
        internal PublisherBsonConfiguration()
        {
            BsonClassMap.RegisterClassMap<Publisher>(cm =>
                {
                    cm.AutoMap();
                    cm.GetMemberMap(c => c.Id).SetSerializer(new IntToGuidBsonSerializer());
                });
        }
    }
}
