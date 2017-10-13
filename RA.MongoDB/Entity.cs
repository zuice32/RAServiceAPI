using System;
using MongoDB.Bson;

namespace RA.MongoDB
{
    public class Entity : IEntity<string>
    {
        public string Id { get; set; }

        public BsonDocument serializedInfo { get; set; }
    }
}
