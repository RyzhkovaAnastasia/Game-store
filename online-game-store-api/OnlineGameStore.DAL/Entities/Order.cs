using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OnlineGameStore.Common.Enums;
using OnlineGameStore.DAL.BsonSerializers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineGameStore.DAL.Entities
{
    public class Order : BaseEntity
    {
        [BsonElement("OrderID")]
        [BsonSerializer(typeof(IntToGuidBsonSerializer))]
        public override Guid Id { get; set; }
        [NotMapped]
        [BsonElement("CustomerID")]
        public string MongoCustomerId { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        [NotMapped]
        [BsonElement("EmployeeID")]
        public int? EmployeeId { get; set; }

        [BsonSerializer(typeof(DateTimeBsonSerializer))]
        public DateTime? OrderDate { get; set; }

        [NotMapped]
        [BsonSerializer(typeof(DateTimeBsonSerializer))]
        public DateTime? RequiredDate { get; set; }

        [BsonSerializer(typeof(DateTimeBsonSerializer))]
        public DateTime? ShippedDate { get; set; }
        public OrderState? OrderState { get; set; }

        [BsonElement("ShipVia")]
        [BsonSerializer(typeof(IntToNullableGuidBsonSerializer))]
        public Guid? ShipperId { get; set; }

        [BsonSerializer(typeof(NullableDoubleBsonSerializer))]
        public double? Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }

        [BsonSerializer(typeof(MixedToStringBsonSerializer))]
        public string ShipRegion { get; set; }

        [BsonSerializer(typeof(MixedToStringBsonSerializer))]
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }
        public virtual ICollection<OrderDetail> Items { get; set; }
        public Order()
        {
            OrderState = Common.Enums.OrderState.Opened;
        }
    }
}
