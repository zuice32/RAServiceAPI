using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

namespace RA.MongoDB
{    
    public abstract class Entity : IEntity
    {
        [BsonId(IdGenerator = typeof(CounterIdGenerator))]
        [BsonRepresentation(BsonType.Int32)]
        [BsonIgnoreIfDefault]
        public int Id { get; set; }        
    }

    public class CounterIdGenerator : IIdGenerator
    {
        private static int _counter = 0;
        public object GenerateId(object container, object document)
        {
            return _counter++;
        }

        public bool IsEmpty(object id)
        {
            return id.Equals(default(int));
        }
    }
}
