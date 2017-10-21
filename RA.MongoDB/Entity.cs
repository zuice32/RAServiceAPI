using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;

namespace RA.MongoDB
{    
    public abstract class Entity : IEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }
    }
}
