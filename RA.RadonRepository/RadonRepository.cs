using System;
using System.Data;
using System.IO;
using Core.Application;
using RA.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using Core.Logging;

namespace RA.RadonRepository
{
    public class RadonRepository : MongoRepository<RadonRepository>
    {
        private int _addOperationsCount;
        private readonly int _addOperationsPerTableTrim;
        private bool _enforceMaxSize = false;

        

        public RadonRepository(
            ICoreIdentity coreIdentity,
            ILogWriter applicationLog) : base(GetLogFileName(coreIdentity), applicationLog)
        {
            //TODO
        }

        protected override string ClassName
        {
            get { return "RA.RadonRepository"; }
        }

        private static string GetLogFileName(ICoreIdentity coreIdentity)
        {
            return Path.Combine(coreIdentity.PathToCoreDataDirectory, "main_" + coreIdentity.ID + ".log");
        }

        public void Initialize()
        {
            base.InitializeBase();

            this.VerifyRadonCollection();
        }

        public async void VerifyRadonCollection()
        {
            bool recordsExists;

            var filter = new BsonDocument("name", "RadonRecords");
            var collectionCursor = await base.Context.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter });
            recordsExists = await collectionCursor.AnyAsync();

            if (!recordsExists)
            {
                this.CreateRadonCollection(base.Context);
            }
        }

        private void CreateRadonCollection(IMongoDatabase dbConnection)
        {
            dbConnection.CreateCollection("RadonRecords");
        }

        public void Add(IRadonRecord entry)
        {
            base.CheckInitialized();


            base.Context.GetCollection<IRadonRecord>("RadonRecords").InsertOne(entry);


            if (_enforceMaxSize) EnforceMaxRadonSize();
        }

        private void EnforceMaxRadonSize()
        {
            _addOperationsCount++;

            if (_addOperationsCount > _addOperationsPerTableTrim)
            {
                _addOperationsCount = 0;

                base.CheckInitialized();

                DeleteResult result = BuildTrimCommand();

                int rowsTrimmed = (int)result.DeletedCount;

                base.ApplicationLog.AddEntry(
                    new LogEntry(
                        MessageLevel.Verbose,
                        ClassName,
                        rowsTrimmed + " rows trimmed from Log table.",
                        DateTime.Now));

            }
        }

        protected virtual DeleteResult BuildTrimCommand()
        {
            var coll = base.Context.GetCollection<IRadonRecord>("RadonRecords");

            var query =
                coll.AsQueryable()
                .OrderByDescending(e => e.radon_data_identifier)
                .Select(e => e).Take(9999999);


            return coll.DeleteMany(query.ToBsonDocument());
        }
    }
}
