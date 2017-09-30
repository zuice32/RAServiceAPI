using System;
using System.Globalization;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Core.Application;
using Core.Logging;
using Core.MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using Newtonsoft.Json;

namespace TA.MongoDB
{
    public class TAMongoDBRepository : MongoDBRepository<IEntity>
    {
        public string _tableName;
        public string _createTableSql;
        public Dictionary<string, string> _createColumnSql;

        public TAMongoDBRepository(ICoreIdentity identity, ILogWriter applicationLog)
            : base(GetDbFileName(identity), applicationLog)
        {
        }

        private static string GetDbFileName(ICoreIdentity coreIdentity)
        {
            return Path.Combine(coreIdentity.PathToCoreDataDirectory, "main_" + coreIdentity.ID + ".happyw");
        }

        protected override string ClassName
        {
            get { return "TA.MongoDB.TAMongoDBRepository"; }
        }

        public void Initialize()
        {
            base.InitializeBase();
            BsonDocument nested = new BsonDocument { };
            this.VerifyTable(_tableName, nested);
        }

        private new void VerifyTable(string tableName, BsonValue instance)
        {
            base.VerifyTable(tableName, instance);
        }

        public void UpsertEntity(IEntity entity, string table) 
        {
            
        }

        public IEnumerable<Entity> ListEntities(string table) 
        {
            base.CheckInitialized();

            List<Entity> entities = new List<Entity>();

            entities.AddRange(base.Context.GetCollection<Entity>(table).AsQueryable().ToList());

            return entities.ToArray();
        }

        public void DeleteEntity(IEntity entity, string table)
        {
            
        }
    }
}