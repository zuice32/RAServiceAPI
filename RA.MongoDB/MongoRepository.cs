using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace RA.MongoDB
{
    public abstract class MongoRepository<TEntity, TIdentifier> : IMongoRepository<TEntity, TIdentifier> where TEntity : class, IEntity
    {
        IMongoDatabase database;

        IMongoCollection<TEntity> _collection;

        public IMongoCollection<TEntity> Collection { get { return _collection; } }

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

            _collection = database.GetCollection<TEntity>(typeof(TEntity).Name.ToLower());
            //var client = new MongoClient(settings.Value.ConnectionString);
            //if (client != null)
            //    database = client.GetDatabase(settings.Value.Database);

        }

        private IQueryable<TEntity> CreateSet()
        {
            return _collection.AsQueryable<TEntity>();
        }

        public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            var set = CreateSet();
            return set.Any(predicate);
        }

        public virtual IReadOnlyList<TEntity> List(Expression<Func<TEntity, bool>> condition = null, Func<TEntity, string> order = null)
        {
            var set = CreateSet();
            if (condition != null)
            {
                set = set.Where(condition);
            }

            if (order != null)
            {
                return set.OrderBy(order).ToList();
            }

            return set.ToList();
        }

        public virtual TEntity Get(TIdentifier id)
        {
            return this.database.GetCollection<TEntity>(typeof(TEntity).Name).Find(x => x.Id.Equals(id)).FirstOrDefaultAsync().Result;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return this.database.GetCollection<TEntity>(typeof(TEntity).Name).AsQueryable();
        }

        public virtual IMongoCollection<TEntity> Find(string collection)
        {
            // Return the enumerable of the collection
            if (_collection==null) _collection = database.GetCollection<TEntity>(collection);

            return _collection;
            //return await _collection.Find<TEntity>(query).ToListAsync();
        }


        public virtual TEntity Save(TEntity entity)
        {
            var collection = this.database.GetCollection<TEntity>(typeof(TEntity).Name);

            collection.InsertOneAsync(entity);

            return entity;
        }

        public virtual IEnumerable<TEntity> InsertCollection(IEnumerable<TEntity> entities)
        {
            var collection = this.database.GetCollection<TEntity>(typeof(TEntity).Name);

            collection.InsertMany(entities);

            return entities;
        }

        public virtual void Delete(TIdentifier id)
        {
            var collection = this.database.GetCollection<TEntity>(typeof(TEntity).Name);

            collection.DeleteOneAsync(x => x.Id.Equals(id));
        }

        public virtual void Delete(TEntity entity)
        {
            var collection = this.database.GetCollection<TEntity>(typeof(TEntity).Name);

            collection.DeleteOneAsync(x => x.Id.Equals(entity.Id));
        }
    }
}
