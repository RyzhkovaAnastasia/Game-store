using MongoDB.Bson.Serialization;
using OnlineGameStore.DAL.BsonSerializers;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.DAL.Configurations.EntitiesBsonConfigurations
{
    internal class ShipperBsonConfiguration
    {
        internal ShipperBsonConfiguration()
        {
            BsonClassMap.RegisterClassMap<Shipper>(cm =>
            {
                cm.AutoMap();
                cm.GetMemberMap(c => c.Id).SetSerializer(new IntToGuidBsonSerializer());
            });
        }
    }
}
