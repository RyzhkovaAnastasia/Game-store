using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;

namespace OnlineGameStore.DAL.BsonSerializers
{
    internal class DateTimeBsonSerializer : SerializerBase<DateTime?>
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTime? value)
        {
            context.Writer.WriteString(value.GetValueOrDefault().ToString());
        }

        public override DateTime? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            IBsonReader bsonReader = context.Reader;
            BsonType type = bsonReader.CurrentBsonType;

            if (type != BsonType.String)
            {
                return null;
            }
            string value = bsonReader.ReadString();
            if (value == "NULL")
            {
                return null;
            }
            DateTime date = DateTime.ParseExact(value, "yyyy-MM-dd h:mm:ss.fff", null);
            return DateTime.SpecifyKind(date, DateTimeKind.Utc);
        }
    }
}
