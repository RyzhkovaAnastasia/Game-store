using MongoDB.Bson.Serialization;
using OnlineGameStore.DAL.BsonSerializers;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.DAL.Configurations.EntitiesBsonConfigurations
{
    internal class OrderDetailBsonConfiguration
    {
        internal OrderDetailBsonConfiguration()
        {
            BsonClassMap.RegisterClassMap<OrderDetail>(cm =>
            {
                cm.AutoMap();
                cm.GetMemberMap(c => c.Discount).SetSerializer(new DiscountBsonSerializer());
                cm.GetMemberMap(c => c.OrderId).SetSerializer(new IntToGuidBsonSerializer());
                cm.GetMemberMap(c => c.GameId).SetSerializer(new IntToGuidBsonSerializer());
            });
        }
    }
}
