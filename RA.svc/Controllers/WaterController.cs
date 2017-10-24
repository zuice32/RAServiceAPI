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
            if (_waterRepo.WaterModelsExist(wd => wd.year==year))
            {

            }


            List<WaterRecord> coll = new List<WaterRecord>();
            using (Client<WaterRecord> client = new Client<WaterRecord>(_url))
            {
                if (await client.CheckConnection())
                {
                    coll = client.Get("?$select=count(*),%20characteristic_name,%20median(result_measure_value)%20as%20median&$where=characteristic_name%20in%20(%27Dissolved%20oxygen%20(DO)%27,%27pH%27,%27Total%20suspended%20solids%27,%27Fecal%20Streptococcus%20Group%20Bacteria%27,%27Nitrogen%27,%20%27Phosphorus%27,%27Iron%27,%27Manganese%27,%20%27Sulfate%27,%20%27Specific%20conductance%27,%27Chloride%27)"+
                        "%20and%20date_extract_y(activity_start_date)%20%3E=%20%27"+year+"%27%20and%20within_circle(location,%20"+ latitude + ",%20"+longitude+",%2010000)%20and%20result_measure_value%20is%20not%20null%20and%20method_qualifier_type_name%20in(%27Routine%20Sampling%27,%20%27Ambient%20Sampling%27)%20and%20activity_type_code%20!=%20%27Quality%20Control%20Sample-Field%20Blank%27&$order=characteristic_name&$group=%20characteristic_name").ToList();
                }
            }
            

            if (coll.Count() > 0)
            {
                List<WaterModel> models = new List<WaterModel>();

                foreach (WaterRecord rec in coll)
                {
                    models.Add(new WaterModel()
                    {
                        type = "water",
                        characteristic = rec.characteristic_name,
                        count = rec.count,
                        //since the amount is calculated in ug/L we convert to mg/L
                        median = rec.characteristic_name == "Iron" || rec.characteristic_name == "Manganese" ?
                            Math.Round(rec.median * 0.001, 2) : Math.Round(rec.median, 2),
                        year = year
                    });
                }
                //somereason this fails
                //_waterRepo.SaveWaterModels(models);
                return Ok(models);
                //HttpResponseMessage response = Response.CreateResponse(HttpStatusCode.OK, model);
            }
            else
            {
                return NotFound(new { message = "Water not found" });
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
