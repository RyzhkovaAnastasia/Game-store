using MongoDB.Bson.Serialization.Attributes;
using OnlineGameStore.DAL.BsonSerializers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace OnlineGameStore.DAL.Entities
{
    public class Game : BaseEntity
    {
        [BsonElement("ProductID")]
        [BsonSerializer(typeof(IntToGuidBsonSerializer))]
        public override Guid Id { get; set; }

        private string key;
        public string Key
        {
            get => key;
            set => key = Regex.Replace(value ?? Name, @"[^0-9a-zA-Z]+", "-").ToLower();
        }

        [BsonElement("CategoryID")]
        [NotMapped]
        [BsonSerializer(typeof(IntToNullableGuidBsonSerializer))]
        public Guid? MongoGenreId { get; set; }


        [BsonElement("ProductName")]
        public string Name { get; set; }

        [BsonElement("QuantityPerUnit")]
        public string Description { get; set; }
        public int ViewsNumber { get; set; }
        public DateTime Published { get; set; }
        public int UnitsOnOrder { get; set; }
        public int ReorderLevel { get; set; }
        public string DownloadPath { get; set; }

        private decimal price;
        [BsonElement("UnitPrice")]
        public decimal Price
        {
            get => Math.Round(price, 2);
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Price must be grater than 0");
                }
                else
                {
                    price = Math.Round(value, 2);
                }
            }
        }

        private short unitsInStock;
        public short UnitsInStock
        {
            get => unitsInStock;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Units in stock must be grater than 0");
                }
                else
                {
                    unitsInStock = value;
                }
            }
        }

        private short discount;
        public short Discount
        {
            get => discount;
            set
            {
                if (value < 0 && value > 100)
                {
                    throw new ArgumentException("Discount must be in 0 - 100 range");
                }
                else
                {
                    discount = value;
                }
            }
        }

        public bool Discontinued { get; set; }


        [BsonElement("SupplierID")]
        [BsonSerializer(typeof(IntToNullableGuidBsonSerializer))]
        public Guid? PublisherId { get; set; }
        public virtual Publisher Publisher { get; set; }

        public virtual ICollection<GameGenre> Genres { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<GamePlatformType> PlatformTypes { get; set; }
    }
}