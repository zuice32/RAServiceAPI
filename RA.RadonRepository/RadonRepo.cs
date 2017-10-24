using System;
using System.Data;
using System.IO;
using Core.Application;
using RA.MongoDB;
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


namespace RA.RadonRepository
{
    public class RadonRepo : MongoRepository<RadonModel, string>, IRadonRepo
    {
        private int _addOperationsCount;

        protected IMongoCollection<RadonModel> _collection;

        //private readonly int _addOperationsPerTableTrim;
        //private bool _enforceMaxSize = false;

        public RadonRepo(IOptions<Settings> setting) : base(setting)
        {
            _collection = base.Collection;
        }


        //public RadonRepository(
        //    ICoreIdentity coreIdentity,
        //    ILogWriter applicationLog) : base(GetLogFileName(coreIdentity), applicationLog)
        //{
        //    //TODO
        //}

        //protected override string ClassName
        //{
        //    get { return "RA.RadonRepository"; }
        //}

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
        //    _collection = base.Collection;
        //}

        //private void CreateRadonCollection()
        //{
        //    base.database.CreateCollection("RadonModel");
        //}

        public RadonModel GetRadonRecord(string id)
        {
            return base.Get(id);
        }

        public List<RadonModel> findRadon(Expression<Func<RadonModel, bool>> query, string collection)
        {
            IMongoCollection<RadonModel> coll = base.Find(collection);
            return coll.AsQueryable().Where(query).ToList();
        }

        public IEnumerable<RadonModel> GetAllRadonModel()
        {
            return base.GetAll();
        }

        public RadonModel SaveRadonModel(RadonModel entity)
        {
            return base.Save(entity);
        }

        public IEnumerable<RadonModel> SaveRadonModels(List<RadonModel> entities)
        {
            return base.InsertCollection(entities);
        }

        public void DeleteRadonModel(string id)
        {
            base.Delete(id);
        }

        public void DeleteRadonModel(RadonModel entity)
        {
            base.Delete(entity);
        }

        




        private void EnforceMaxRadonSize()
        {
            _addOperationsCount++;

            //if (_addOperationsCount > _addOperationsPerTableTrim)
            //{
                //_addOperationsCount = 0;

                //base.CheckInitialized();

                //DeleteResult result = BuildTrimCommand();

                //int rowsTrimmed = (int)result.DeletedCount;

                //base.ApplicationLog.AddEntry(
                //    new LogEntry(
                //        MessageLevel.Verbose,
                //        ClassName,
                //        rowsTrimmed + " rows trimmed from Log table.",
                //        DateTime.Now));

            //}
        }

        //protected virtual DeleteResult BuildTrimCommand()
        //{
        //    var coll = base.Context.GetCollection<IRadonRecord>("RadonRecords");

        //    var query =
        //        coll.AsQueryable()
        //        .OrderByDescending(e => e.radon_data_identifier)
        //        .Select(e => e).Take(9999999);


        //    return coll.DeleteMany(query.ToBsonDocument());
        //}
    }
}
