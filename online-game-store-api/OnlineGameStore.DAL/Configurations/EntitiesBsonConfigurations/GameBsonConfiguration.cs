using MongoDB.Bson.Serialization;
using OnlineGameStore.DAL.BsonSerializers;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.DAL.Configurations.EntitiesBsonConfigurations
{
    internal class GameBsonConfiguration
    {
        internal GameBsonConfiguration()
        {
            BsonClassMap.RegisterClassMap<Game>(cm =>
                {
                    cm.AutoMap();
                    cm.GetMemberMap(c => c.PublisherId).SetSerializer(new IntToNullableGuidBsonSerializer());
                    cm.GetMemberMap(c => c.MongoGenreId).SetSerializer(new IntToNullableGuidBsonSerializer());
                    cm.GetMemberMap(c => c.Id).SetSerializer(new IntToGuidBsonSerializer());
                });

        }
    }
}
