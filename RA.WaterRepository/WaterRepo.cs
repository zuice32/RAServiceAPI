using System;
using System.Data;
using System.IO;
using Core.Application;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using MongoDB;
using Core.Logging;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RA.MongoDB;


namespace RA.WaterRepository
{
    public class WaterRepo : MongoRepository<WaterModel, string>, IWaterRepo
    {
        private int _addOperationsCount;

        protected IMongoCollection<WaterModel> _collection;

        //private readonly int _addOperationsPerTableTrim;
        //private bool _enforceMaxSize = false;

        public WaterRepo(IOptions<Settings> setting) : base(setting)
        {
        }

        private static string GetLogFileName(ICoreIdentity coreIdentity)
        {
            return Path.Combine(coreIdentity.PathToCoreDataDirectory, "main_" + coreIdentity.ID + ".log");
        }

        public void Initialize()
        {
            //base.InitializeBase();

            //this.VerifyRadonCollection();
        }

        //public void VerifyRadonCollection()
        //{
        //    _collection = base.database.GetCollection<WaterModel>("WaterModel");
        //}

        //private void CreateWaterCollection()
        //{
        //    base.database.CreateCollection("WaterModel");
        //}

        public bool WaterModelsExist(Expression<Func<WaterModel, bool>> predicate) {
            return base.Exists(predicate);
        }

        public WaterModel GetWaterRecord(string id)
        {
            return base.Get(id);
        }

        public List<WaterModel> findWaterModels(Expression<Func<WaterModel, bool>> query, string collection)
        {
            IMongoCollection<WaterModel> coll = base.Find(collection);
            return coll.AsQueryable().Where(query).ToList();
        }

        public IEnumerable<WaterModel> GetAllWaterModels()
        {
            return base.GetAll();
        }

        public WaterModel SaveWaterModel(WaterModel entity)
        {
            return base.Save(entity);
        }

        public IEnumerable<WaterModel> SaveWaterModels(List<WaterModel> entities)
        {
            return base.InsertCollection(entities);
        }

        public void DeleteWaterModel(string id)
        {
            base.Delete(id);
        }

        public void DeleteWaterModel(WaterModel entity)
        {
            base.Delete(entity);
        }
    }
}
