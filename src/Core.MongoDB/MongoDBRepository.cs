using System;
using MongoDB.Driver;
using MongoDB.Bson;
using System.IO;
using Core.Logging;

namespace Core.MongoDB
{
    public abstract class MongoDBRepository<T1> : IDisposable where T1 : class
    {
        private readonly object _syncLock = new object();
        private readonly string _pathToDbFile;
        public static IMongoClient _client;
        public static IMongoDatabase context;
        private bool disposed = false;

        protected MongoDBRepository(string pathToDbFile, ILogWriter applicationLog)
        {
            _pathToDbFile = pathToDbFile;

            this.ApplicationLog = applicationLog;

            Context = GetConnection(_pathToDbFile);
        }

        protected ILogWriter ApplicationLog { get; private set; }

        protected bool IsInitialized { get; private set; }

        protected IMongoDatabase Context
        {
            get
            {
                return context;
            }
            set
            {
                context = value;
            }
        }

        protected abstract string ClassName { get; }

        protected virtual IMongoDatabase GetConnection(string pathToDbFile)
        {
            string connectionString = string.Format(
                "Data Source={0};Version=3;PRAGMA integrity_check=10;foreign keys=true;",
                pathToDbFile);

            _client = new MongoClient(connectionString);

            return _client.GetDatabase("API");
        }

        protected virtual void InitializeBase()
        {
            lock (_syncLock)
            {
                if(this.IsInitialized) return;

                this.IsInitialized = true;

                if (!File.Exists(_pathToDbFile))
                {
                    CreateNewDb(_pathToDbFile);

                    this.ApplicationLog.AddEntry(
                        new LogEntry(
                            MessageLevel.AppLifecycle,
                            ClassName,
                            "New database file created at " + _pathToDbFile,
                            DateTime.Now));
                }

                VerifyDb(_pathToDbFile, this.ApplicationLog);
            }
        }

        protected void VerifyTable(string tableName, BsonValue instance)
        {
            var filter = new BsonDocument(tableName, instance);
            //filter by collection name
            var collections = GetConnection(_pathToDbFile).ListCollections(new ListCollectionsOptions { Filter = filter });
            //check for existence
            if (!collections.Any()) {

                Context.CreateCollection(tableName);

            }
        }

        protected void VerifyIndex(string tableName, string createIndexTime)
        {
            if (!context.GetCollection<T1>(tableName).Indexes.List().Any())
            {
                //context.GetCollection<T1>(tableName).Indexes.
            }
        }

        protected void CreateTableCollection(string tableName) {

            context.CreateCollection(tableName);

        }

        protected bool VerifyColumn(string tableName, string columnName)
        {
            var fieldValueIsNullFilter = Builders<T1>.Filter.Eq(columnName, BsonNull.Value);
            return context.GetCollection<T1>(tableName).Count(fieldValueIsNullFilter) == 0;                       
        }

        protected void CheckInitialized()
        {
            lock (_syncLock)
            {
                if (!this.IsInitialized)
                {
                    throw new InvalidOperationException(
                        "'Initialize' method must be called before using an instance of this class.");
                }

                Context = GetConnection(_pathToDbFile);
            }
        }

        protected virtual void CreateNewDb(string pathToDbFile)
        {
            string connectionString = string.Format(
                "Data Source={0};Version=3;PRAGMA integrity_check=10;foreign keys=true;",
                pathToDbFile);

            _client = new MongoClient(connectionString);

            this.Context = _client.GetDatabase("API");
        }

        protected virtual void VerifyDb(string pathToDbFile, ILogWriter applicationLog)
        {
            try
            {
                if (Context == null)
                {
                    Context = this.GetConnection(pathToDbFile);

                    bool exists = Context.ListCollections().Any();
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }

        private void ReplaceDb(string pathToDbFile, ILogWriter applicationLog, Exception exception)
        {
            string backupFileName = string.Format(
                "{0}_{1}.invalid",
                pathToDbFile,
                DateTime.UtcNow.ToString("yyyy-MM-ddThh-mm-ss"));

            File.Copy(pathToDbFile, backupFileName);

            File.Delete(pathToDbFile);

            this.CreateNewDb(pathToDbFile);

            applicationLog.AddEntry(
                new LogEntry(
                    MessageLevel.Warning,
                    ClassName,
                    string.Format(
                        "Invalid database file found at {0}. Replacement file created.{1}Error:{2}",
                        pathToDbFile,
                        Environment.NewLine,
                        exception.Message),
                    DateTime.Now));
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //MongoDB client controls disposal
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}