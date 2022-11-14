using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;

namespace OnlineGameStore.DAL.BsonSerializers
{
    internal class DiscountBsonSerializer : SerializerBase<float>
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, float value)
        {
            context.Writer.WriteDouble(Math.Round(value / 100, 2));
        }

        public override float Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            IBsonReader bsonReader = context.Reader;
            if (bsonReader.CurrentBsonType == MongoDB.Bson.BsonType.Int32)
            {
                int intValue = bsonReader.ReadInt32();
                return intValue * 100.0f;
            }
            double doubleValue = bsonReader.ReadDouble();
            return (float)(doubleValue * 100.0f);
        }
    }
}
