using MongoDB.Driver;
using MongoDB.Bson;
using Core.Application;
using Core.Logging;
using Core.Logging.MongoDB;

namespace Core.Logging.MongoDB
{
    internal class MongoDBLogRepositoryDecorator : MongoDBLogRepository
    {
        public MongoDBLogRepositoryDecorator(ICoreIdentity coreIdentity, ILogWriter applicationLog)
            : this(coreIdentity, applicationLog, 1000000)
        {
        }

        public MongoDBLogRepositoryDecorator(ICoreIdentity coreIdentity,
            ILogWriter applicationLog,
            int maxEntryCount)
            : base(coreIdentity, applicationLog, maxEntryCount)
        {
        }

        public IMongoDatabase GetNewDbConnection()
        {
            base.CheckInitialized();

            return base.Context;
        }
    }
}
