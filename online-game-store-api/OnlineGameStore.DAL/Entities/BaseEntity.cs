using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineGameStore.DAL.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public virtual Guid Id { get; set; }

        [BsonId]
        [NotMapped]
        public ObjectId ObjectId { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public BaseEntity()
        {
            Created = Updated = DateTime.UtcNow;
        }
    }
}
