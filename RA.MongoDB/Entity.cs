using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;

namespace RA.MongoDB
{    
    public abstract class Entity : IEntity
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid Id { get; set; }
    }
}
