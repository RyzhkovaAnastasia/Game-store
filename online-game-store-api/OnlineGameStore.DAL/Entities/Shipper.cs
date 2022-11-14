using MongoDB.Bson.Serialization.Attributes;
using OnlineGameStore.DAL.BsonSerializers;
using System;

namespace OnlineGameStore.DAL.Entities
{
    public class Shipper : BaseEntity
    {
        [BsonElement("ShipperID")]
        [BsonSerializer(typeof(IntToGuidBsonSerializer))]
        public override Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
    }
}
