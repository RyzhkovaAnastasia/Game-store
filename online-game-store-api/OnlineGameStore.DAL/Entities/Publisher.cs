using MongoDB.Bson.Serialization.Attributes;
using OnlineGameStore.DAL.BsonSerializers;
using System;

namespace OnlineGameStore.DAL.Entities
{
    public class Publisher : BaseEntity
    {
        [BsonElement("SupplierID")]
        [BsonSerializer(typeof(IntToGuidBsonSerializer))]
        public override Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string HomePage { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
    }
}
