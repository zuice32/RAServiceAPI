using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB;
using MongoDB.Driver;
using MongoDB.Bson;
using RA.microservice.Interface;
using RA.microservice.Model;
using Microsoft.Extensions.Options;

namespace RA.MongoDB
{
    public class MongoRepository<TEntity, TIdentifier> : IMongoRepository<TEntity, TIdentifier> where TEntity : class, IEntity
    {
        public IMongoDatabase database;

        public MongoRepository(IOptions<Settings> settings)
        {

            //for remote (mlab) connect requires a little more elbow greese
            var credential = MongoCredential.CreateCredential(settings.Value.Database, settings.Value.username, settings.Value.pass);

            var mongoClientSettings = new MongoClientSettings
            {
                Server = new MongoServerAddress(settings.Value.url, settings.Value.port),
                Credentials = new List<MongoCredential> { credential }
            };
            var client = new MongoClient(mongoClientSettings);
            if (client != null)
                database = client.GetDatabase(settings.Value.Database);

#if Debug
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                database = client.GetDatabase(settings.Value.Database);
#endif
        }

        public TEntity Get(TIdentifier id)
        {
            return this.database.GetCollection<TEntity>(typeof(TEntity).Name).Find(x => x.Id.Equals(id)).FirstOrDefaultAsync().Result;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return this.database.GetCollection<TEntity>(typeof(TEntity).Name).Find(new BsonDocument()).ToListAsync().Result;
        }


        public TEntity Save(TEntity entity)
        {
            var collection = this.database.GetCollection<TEntity>(typeof(TEntity).Name);

            collection.ReplaceOneAsync(x => x.Id.Equals(entity.Id), entity, new UpdateOptions
            {
                IsUpsert = true
            });

            return entity;
        }

        public IEnumerable<TEntity> InsertCollection(IEnumerable<TEntity> entities)
        {
            var collection = this.database.GetCollection<TEntity>(typeof(TEntity).Name);

            collection.InsertMany(entities);

            return entities;
        }

        public void Delete(TIdentifier id)
        {
            var collection = this.database.GetCollection<TEntity>(typeof(TEntity).Name);

            collection.DeleteOneAsync(x => x.Id.Equals(id));
        }

        public void Delete(TEntity entity)
        {
            var collection = this.database.GetCollection<TEntity>(typeof(TEntity).Name);

            collection.DeleteOneAsync(x => x.Id.Equals(entity.Id));
        }
    }
}
