using MongoDB.Bson.Serialization.Attributes;
using OnlineGameStore.DAL.BsonSerializers;
using System;
using System.Collections.Generic;

namespace OnlineGameStore.DAL.Entities
{
    public class Genre : BaseEntity
    {
        [BsonElement("CategoryID")]
        [BsonSerializer(typeof(IntToGuidBsonSerializer))]
        public override Guid Id { get; set; }

        [BsonElement("CategoryName")]
        public string Name { get; set; }
        public Guid? ParentGenreId { get; set; }
        public virtual Genre ParentGenre { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public virtual ICollection<Genre> ChildGenres { get; set; } = new List<Genre>();

        public virtual ICollection<GameGenre> Games { get; set; }
    }
}
