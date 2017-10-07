using System;
using MongoDB.Bson;

namespace RA.MongoDB
{
    public interface IEntity
    {
        Guid Id { get; set; }

        BsonDocument serializedInfo { get; set; }
    }
}