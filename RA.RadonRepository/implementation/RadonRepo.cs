using System;
using System.IO;
using System.Linq;
using Core.Application;
using RA.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RA.DALAccess;

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

        public async Task<RadonModel> createRadonModel(string url, string zip, int year)
        {
            var model = new RadonModel();
            List<RadonRecord> coll = new List<RadonRecord>();
            using (Client<RadonRecord> client = new Client<RadonRecord>(url))
            {
                if (await client.CheckConnection())
                {
                    coll = client.Get("?address_postal_code=" + zip).ToList();
                }
            }

            var curr = coll.Where(rr => rr.test_start_date.Year == year || rr.test_end_date.Year == year).ToList();
            
            var avg = curr.Count > 0 ? curr.Average(ra => ra.measure_value) : 0;
            var min = curr.Count > 0 ? curr.Min(ra => ra.measure_value) : 0;
            var max = curr.Count > 0 ? curr.Max(ra => ra.measure_value) : 0;
            
            model = new RadonModel()
            {
                type = "radon",
                zip = zip,
                minValue = min,
                maxValue = max,
                numberOfTests = (uint)curr.Count(),
                average = Math.Round(avg, 2),

                median = curr.Count > 0 ? curr.OrderBy(ra => ra.measure_value)
                .ElementAt(curr.Count() / 2)
                .measure_value + curr.ElementAt((curr.Count() - 1) / 2)
                .measure_value : 0,

                year = (uint)year,

                year_data = coll.Where(rr => rr.test_end_date.Year >= 1990)
                .GroupBy(rr => rr.test_end_date.Year)
                .Select(rr => (uint)rr.Key)
                .ToList(),

                count_data = coll.Where(rr => rr.test_end_date.Year >= 1990)
                .GroupBy(rr => rr.test_end_date.Year)
                .Select(rr => (uint)rr.Count())
                .ToList(),
                
                average_data = coll.Where(rr => rr.test_end_date.Year >= 1990)
                .GroupBy(rr => rr.test_end_date.Year)
                .Select(rr =>
                    Math.Round(rr.Average(m => m.measure_value), 2)
                ).ToList()
                //averageColor
            };


            this.SaveRadonModel(model);

            if (curr.Count == 0 && coll.Count == 0)
            {
                return null;
            }
            else
            {
                return model;
            }            
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
