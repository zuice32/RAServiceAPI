using System;
using MongoDB.Bson;

namespace RA.MongoDB
{
    public class Entity : IEntity
    {
        public Guid Id { get; set; }

        public BsonDocument serializedInfo { get; set; }
    }
}
