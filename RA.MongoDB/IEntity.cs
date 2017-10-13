using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RA.MongoDB
{
    public interface IEntity<Identity>
    {
        Identity Id { get; set; }

        BsonDocument serializedInfo { get; set; }
    }
}