using System;
using System.Data;
using System.IO;
using Core.Application;
using Core.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson;

namespace Core.Logging.MongoDB
{
    public class MongoDBLogRepository : MongoDBRepository<MongoDBLogRepository>, ILogRepository
    {
        private int _addOperationsCount;
        private readonly int _addOperationsPerTableTrim;
        private readonly int _maxEntryCount;

        public MongoDBLogRepository(ICoreIdentity coreIdentity, ILogWriter applicationLog)
            : this(coreIdentity, applicationLog, 100000)
        {
        }

        public MongoDBLogRepository(
            ICoreIdentity coreIdentity,
            ILogWriter applicationLog,
            int maxEntryCount) : base(GetLogFileName(coreIdentity), applicationLog)
        {
            _maxEntryCount = maxEntryCount;

            _addOperationsPerTableTrim = Math.Max(100, maxEntryCount/100);
        }

        protected override string ClassName
        {
            get { return "Core.Logging.MongoDB.MongoDBLogRepository"; }
        }

        private static string GetLogFileName(ICoreIdentity coreIdentity)
        {
            return Path.Combine(coreIdentity.PathToCoreDataDirectory, "main_" + coreIdentity.ID + ".log");
        }

        protected virtual DeleteResult BuildTrimTableCommand()
        {
            var coll = base.Context.GetCollection<ILogEntry>("Logging");

            var query =
                coll.AsQueryable()
                .OrderByDescending(e => e.Time)
                .Select(e => e).Take(_maxEntryCount);


            return coll.DeleteMany(query.ToBsonDocument());
        }

        public void Initialize()
        {
            base.InitializeBase();

            this.VerifyLogTable();
        }

        public async void VerifyLogTable()
        {
            bool logTableExists;
            
            var filter = new BsonDocument("name", "Logging");
            var collectionCursor = await base.Context.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter });
            logTableExists = await collectionCursor.AnyAsync();

            if (!logTableExists)
            {
                this.CreateLogTable(base.Context);
            }
        }

        private void CreateLogTable(IMongoDatabase dbConnection)
        {
            dbConnection.CreateCollection("Logging");
        }

        public void Add(ILogEntry entry)
        {
            base.CheckInitialized();


            base.Context.GetCollection<ILogEntry>("Logging").InsertOne(entry);


            this.EnforceMaxLogSize();
        }

        private void EnforceMaxLogSize()
        {
            _addOperationsCount++;

            if (_addOperationsCount > _addOperationsPerTableTrim)
            {
                _addOperationsCount = 0;

                base.CheckInitialized();

                DeleteResult result = BuildTrimTableCommand();

                int rowsTrimmed = (int)result.DeletedCount;

                base.ApplicationLog.AddEntry(
                    new LogEntry(
                        MessageLevel.Verbose,
                        ClassName,
                        rowsTrimmed + " rows trimmed from Log table.",
                        DateTime.Now));
                        
            }
        }
    }
}