using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace OnlineGameStore.DAL.BsonSerializers
{
    internal class NullableDoubleBsonSerializer : SerializerBase<double?>
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, double? value)
        {
            if (value.HasValue)
            {
                context.Writer.WriteDouble(value.Value);
            }
            else
            {
                context.Writer.WriteNull();
            }
        }

        public override double? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            double? value = null;
            if (context.Reader.CurrentBsonType == BsonType.String && double.TryParse(context.Reader.ReadString(), out double doubleValue))
            {
                value = doubleValue;
            }
            else if (context.Reader.CurrentBsonType == BsonType.Double)
            {
                value = context.Reader.ReadDouble();
            }

            return value;
        }
    }
}
