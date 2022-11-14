using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace OnlineGameStore.DAL.BsonSerializers
{
    public class MixedToStringBsonSerializer : StringSerializer
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, string value)
        {
            context.Writer.WriteString(value);
        }

        public override string Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {

            IBsonReader bsonReader = context.Reader;
            MongoDB.Bson.BsonType type = bsonReader.GetCurrentBsonType();

            if (type == MongoDB.Bson.BsonType.Int32)
            {
                return bsonReader.ReadInt32().ToString();
            }
            string value = bsonReader.ReadString();
            if (value == "NULL")
            {
                return null;
            }
            return value;
        }
    }
}

