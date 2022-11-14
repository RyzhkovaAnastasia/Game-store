using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;

namespace OnlineGameStore.DAL.BsonSerializers
{
    internal class IntToNullableGuidBsonSerializer : SerializerBase<Guid?>
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Guid? value)
        {
            if (value == null)
            {
                context.Writer.WriteNull();
            }
            byte[] bytes = value.Value.ToByteArray();
            int intvalue = BitConverter.ToInt32(bytes, 0);
            context.Writer.WriteInt32(intvalue);
        }

        public override Guid? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            IBsonReader bsonReader = context.Reader;
            int value = bsonReader.ReadInt32();
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return (Guid?)new Guid(bytes);
        }
    }
}
