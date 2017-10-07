using System;
using Core.Logging;

namespace Core.MongoDB
{
    internal class TestMongoDBRepository : MongoDBRepository<TestMongoDBRepository>, IDisposable
    {
        public TestMongoDBRepository(string pathToDbFile, ILogWriter applicationLog)
            : base(pathToDbFile, applicationLog)
        {
        }

        public void Initialize()
        {
            base.InitializeBase();
        }

        protected override string ClassName
        {
            get { return "Core.MongoDB.TestMongoDBRepository"; }
        }
    }
}