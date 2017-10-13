using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB;
using MongoDB.Driver;
using MongoDB.Bson;

namespace RA.MongoDB
{
    public class MongoRepository<TEntity, TIdentifier> : IMongoRepository<TEntity, TIdentifier> where TEntity : class, IEntity<TIdentifier>
    {
        private readonly IMongoDatabase database;

        public MongoRepository(ISetting settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            if (client != null)
                database = client.GetDatabase(settings.Database);
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
