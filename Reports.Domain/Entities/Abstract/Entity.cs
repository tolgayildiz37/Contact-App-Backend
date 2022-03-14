using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Reports.Domain.Entities.Abstract
{
    public abstract class Entity : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; protected set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }

        public Entity()
        {
            CreatedTime = DateTime.Now;
        }

        public Entity Clone()
        {
            return (Entity)this.MemberwiseClone();
        }
    }
}
