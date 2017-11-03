using System;
using System.Linq;
using System.IO;
using Core.Application;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RA.MongoDB;
using RA.DALAccess;

namespace RA.WaterRepository
{
    public class WaterRepo : MongoRepository<WaterModel, string>, IWaterRepo
    {

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

        public IEnumerable<WaterModel> findWaterModels(Expression<Func<WaterModel, bool>> query, string collection)
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

        public async Task<WaterModel> CreateWaterModel(string url, double latitude, double longitude, int year)
        {
            WaterModel model = new WaterModel();

            List<WaterRecord> coll = new List<WaterRecord>();
            using (Client<WaterRecord> client = new Client<WaterRecord>(url))
            {
                if (await client.CheckConnection())
                {
                    coll = client.Get("?$select=result_measure_unit_code,%20%20monitoring_location_name%7C%7C%27%20-%20%27%7C%7Cmonitoring_location_identifier%20as%20location_name,%20latitude,%20longitude,%20count(*)%20as%20count,%20characteristic_name,%20median(result_measure_value)%20as%20median&$where=characteristic_name%20in%20(%27Dissolved%20oxygen%20(DO)%27,%27pH%27,%27Total%20suspended%20solids%27,%27Fecal%20Streptococcus%20Group%20Bacteria%27,%27Nitrogen%27,%20%27Phosphorus%27,%27Iron%27,%27Manganese%27,%20%27Sulfate%27,%20%27Specific%20conductance%27,%27Chloride%27)%20and%20date_extract_y(activity_start_date)%20%3E=%20%27"
                    + year + "%27%20and%20within_circle(location,%20" + latitude + ",%20" + longitude + ",%2010000)%20and%20result_measure_value%20is%20not%20null%20and%20method_qualifier_type_name%20in(%27Routine%20Sampling%27,%20%27Ambient%20Sampling%27)%20and%20activity_type_code%20!=%20%27Quality%20Control%20Sample-Field%20Blank%27&$order=characteristic_name&$group=location_name,%20latitude,%20longitude,%20monitoring_location_identity,%20characteristic_name,%20result_measure_unit_code").ToList();
                }
            }


            if (coll.Count() > 0)
            {
                //closes distinct location
                coll = coll.GroupBy(x => Math.Pow((latitude - x.latitude), 2) + Math.Pow((longitude - x.longitude), 2))
                      .OrderBy(x => x.Key)
                      .First().ToList();

                List<WaterCharacteristic> charLst = WaterExtensions.orderedList(coll);

                model = new WaterModel
                {
                    type = "water",
                    characteristics = charLst,
                    data = charLst.Select(cl => cl.median).ToList(),
                    characteristic_data = charLst.Select(cl => cl.characteristic).ToList(),
                    year = year,
                    latitude = latitude,
                    longitude = longitude,
                    location_name = coll.FirstOrDefault().location_name
                };
                
                SaveWaterModel(model);
            }

            return model;
        }
    }
}
