using MongoDB.Bson.Serialization.Attributes;
using OnlineGameStore.DAL.BsonSerializers;
using System;

namespace OnlineGameStore.DAL.Entities
{
    public class OrderDetail : BaseEntity
    {
        [BsonElement("ProductID")]
        [BsonSerializer(typeof(IntToGuidBsonSerializer))]
        public Guid GameId { get; set; }
        public virtual Game Game { get; set; }
        private decimal _price;

        [BsonElement("UnitPrice")]
        public decimal Price
        {
            get => Math.Round(_price, 2);
            set => _price = value;
        }

        private short quantity;
        public short Quantity
        {
            get => quantity;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Price must be grater than 0");
                }
                else
                {
                    quantity = value;
                }
            }
        }

        [BsonSerializer(typeof(DiscountBsonSerializer))]
        public float Discount { get; set; }

        [BsonElement("OrderID")]
        [BsonSerializer(typeof(IntToGuidBsonSerializer))]
        public Guid OrderId { get; set; }
    }
}
