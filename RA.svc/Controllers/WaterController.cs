using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RA.WaterRepository;
using RA.svc.Infrastructure;
using System.Collections;
using RA.DALAccess;
using Newtonsoft.Json;

namespace RA.svc.Controllers
{
    [Produces("application/json")]
    [Route("api/Water")]
    public class WaterController : Controller
    {
        private readonly IWaterRepo _waterRepo;

        public WaterController(IWaterRepo repo)
        {
            _waterRepo = repo;
        }

        private readonly string _url = "http://data.pa.gov/resource/x7jf-72k4.json";

        

        // GET api/values/5
        [NoCache]
        [HttpGet("")]
        public async Task<IActionResult> GetWaterCharacteristicsYear([FromQuery]double latitude, [FromQuery]double longitude, [FromQuery]int year)
        {
            WaterModel init = _waterRepo.GetAllWaterModels().AsQueryable().Where(wd => wd.year == year && wd.latitude == latitude && wd.longitude == longitude).FirstOrDefault();
            if (init != null)
            {
                return Ok(init);
            }
            else {
                List<WaterRecord> coll = new List<WaterRecord>();
                using (Client<WaterRecord> client = new Client<WaterRecord>(_url))
                {
                    if (await client.CheckConnection())
                    {
                        coll = client.Get("?$select=monitoring_location_name%7C%7C%27%20-%20%27%7C%7Cmonitoring_location_identifier%20as%20location_name,%20latitude,%20longitude,%20count(*)%20as%20count,%20characteristic_name,%20median(result_measure_value)%20as%20median&$where=characteristic_name%20in%20(%27Dissolved%20oxygen%20(DO)%27,%27pH%27,%27Total%20suspended%20solids%27,%27Fecal%20Streptococcus%20Group%20Bacteria%27,%27Nitrogen%27,%20%27Phosphorus%27,%27Iron%27,%27Manganese%27,%20%27Sulfate%27,%20%27Specific%20conductance%27,%27Chloride%27)%20and%20date_extract_y(activity_start_date)%20%3E=%20%27"
                        + year + "%27%20and%20within_circle(location,%20" + latitude + ",%20" + longitude + ",%2010000)%20and%20result_measure_value%20is%20not%20null%20and%20method_qualifier_type_name%20in(%27Routine%20Sampling%27,%20%27Ambient%20Sampling%27)%20and%20activity_type_code%20!=%20%27Quality%20Control%20Sample-Field%20Blank%27&$order=characteristic_name&$group=location_name,%20latitude,%20longitude,%20monitoring_location_identity,%20characteristic_name").ToList();
                    }
                }


                if (coll.Count() > 0)
                {
                    coll = coll.GroupBy(x => Math.Pow((latitude - x.latitude), 2) + Math.Pow((longitude - x.longitude), 2))
                          .OrderBy(x => x.Key)
                          .First().ToList();

                    List<WaterCharacteristic> charLst = new List<WaterCharacteristic>();

                    foreach (WaterRecord rec in coll)
                    {
                        charLst.Add(new WaterCharacteristic()
                        {
                            characteristic = rec.characteristic_name,
                            count = rec.count,
                            //since the amount is calculated in ug/L we convert to mg/L
                            median = rec.characteristic_name == "Iron" || rec.characteristic_name == "Manganese" ?
                                Math.Round(rec.median * 0.001, 2) : Math.Round(rec.median, 2)
                        });
                    }

                    WaterModel model = new WaterModel()
                    {
                        type = "water",
                        characteristics = charLst,
                        data = charLst.OrderBy(co => co.characteristic).Select(cl => cl.median).ToList(),
                        characteristic_data = charLst.OrderBy(co => co.characteristic).Select(cl => cl.characteristic).ToList(),
                        year = year,
                        latitude = latitude,
                        longitude = longitude,
                        location_name = coll.FirstOrDefault().location_name
                    };

                    //somereason this fails
                    _waterRepo.SaveWaterModel(model);
                    return Ok(model);
                    //HttpResponseMessage response = Response.CreateResponse(HttpStatusCode.OK, model);
                }
                else
                {
                    return NotFound(new { message = "Water not found" });
                }
            }
        }
        
        
        // POST: api/Water
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Water/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
