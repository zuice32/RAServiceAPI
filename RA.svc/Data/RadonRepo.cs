using System;
using System.Data;
using System.IO;
using Core.Application;
using RA.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using Core.Logging;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using RA.microservice.Interface;
using RA.microservice.Model;
using MongoDB.Bson.Serialization;

namespace RA.RadonRepository
{
    public class RadonRepo : MongoRepository<RadonRecord, string>, IRadonRepo
    {
        private int _addOperationsCount;
        //private readonly int _addOperationsPerTableTrim;
        //private bool _enforceMaxSize = false;


        public RadonRepo(IOptions<Settings> setting) : base(setting)
        { }


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

        public async void VerifyRadonCollection()
        {
            bool recordsExists;

            var filter = new BsonDocument("name", "Radon");
            var collectionCursor = base.GetAll();
            recordsExists = collectionCursor.Equals(null) ? false : true;

            if (!recordsExists)
            {
                this.CreateRadonCollection(base.database);
            }
        }

        private void CreateRadonCollection(IMongoDatabase dbConnection)
        {
            dbConnection.CreateCollection("Radon");
        }

        public RadonRecord GetRadonRecord(string id)
        {
            return base.Get(id);
        }

        public IEnumerable<RadonRecord> GetAllRadonRecords()
        {
            return base.GetAll();
        }

        public RadonRecord SaveRadonRecord(RadonRecord entity)
        {
            return base.Save(entity);
        }

        public IEnumerable<RadonRecord> SaveRadonRecords(List<RadonRecord> entities)
        {
            return base.InsertCollection(entities);
        }

        public void DeleteRadonRecord(string id)
        {
            base.Delete(id);
        }

        public void DeleteRadonRecord(RadonRecord entity)
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
