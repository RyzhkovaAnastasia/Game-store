using MongoDB.Bson.Serialization;
using OnlineGameStore.DAL.BsonSerializers;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.DAL.Configurations.EntitiesBsonConfigurations
{
    internal class OrderBsonConfiguration
    {
        internal OrderBsonConfiguration()
        {
            BsonClassMap.RegisterClassMap<Order>(cm =>
            {
                cm.AutoMap();
                cm.GetMemberMap(c => c.RequiredDate).SetSerializer(new DateTimeBsonSerializer());
                cm.GetMemberMap(c => c.OrderDate).SetSerializer(new DateTimeBsonSerializer());
                cm.GetMemberMap(c => c.ShippedDate).SetSerializer(new DateTimeBsonSerializer());
                cm.GetMemberMap(c => c.ShipPostalCode).SetSerializer(new MixedToStringBsonSerializer());
                cm.GetMemberMap(c => c.Freight).SetSerializer(new NullableDoubleBsonSerializer());
                cm.GetMemberMap(c => c.ShipRegion).SetSerializer(new MixedToStringBsonSerializer());
                cm.GetMemberMap(c => c.ShipperId).SetSerializer(new IntToNullableGuidBsonSerializer());
                cm.GetMemberMap(c => c.Id).SetSerializer(new IntToGuidBsonSerializer());

            });
        }
    }
}
