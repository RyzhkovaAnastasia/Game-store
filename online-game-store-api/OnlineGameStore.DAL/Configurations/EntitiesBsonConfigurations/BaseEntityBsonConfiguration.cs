using MongoDB.Bson.Serialization;
using OnlineGameStore.DAL.BsonSerializers;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.DAL.Configurations.EntitiesBsonConfigurations
{
    internal class BaseEntityBsonConfiguration
    {
        internal BaseEntityBsonConfiguration()
        {
            BsonClassMap.RegisterClassMap<BaseEntity>(cm =>
                {
                    cm.AutoMap();
                    cm.GetMemberMap(c => c.Id).SetSerializer(new IntToGuidBsonSerializer());
                });
        }
    }
}
