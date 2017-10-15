using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace RA.MongoDB
{
    public interface IEntity
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        Guid Id { get; set; }
    }
}