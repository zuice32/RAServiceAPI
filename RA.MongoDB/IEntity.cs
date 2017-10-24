using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace RA.MongoDB
{
    public interface IEntity
    {
        [BsonId]
        int Id { get; set; }
    }
}