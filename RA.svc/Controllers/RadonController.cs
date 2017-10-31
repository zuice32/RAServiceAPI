using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RA.RadonRepository;
using RA.svc.Infrastructure;
using System.Collections;
using RA.DALAccess;
using Newtonsoft.Json;

namespace RA.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class RadonController : Controller
    {
        private readonly IRadonRepo _radonRepo;

        public RadonController(IRadonRepo repo)
        {
            _radonRepo = repo;
        }

        private readonly string _url = "http://data.pa.gov/resource/7ypj-ezu6.json";
        

        // GET api/values/5
        [NoCache]
        [HttpGet("{zip}~{year}")]
        public async Task<IActionResult> GetRadonAverageYear(string zip, int year)
        {
            RadonModel init = _radonRepo.GetAllRadonModel().AsQueryable().Where(rd => rd.year == year && rd.zip == zip).FirstOrDefault();
            if (init != null)
            {
                return Ok(init);
            }
            else
            {
                List<RadonRecord> coll = new List<RadonRecord>();
                using (Client<RadonRecord> client = new Client<RadonRecord>(_url))
                {
                    if (await client.CheckConnection())
                    {
                        coll = client.Get("?address_postal_code=" + zip).ToList();
                    }
                }
                
                var curr = coll.Where(rr => rr.test_start_date.Year == year || rr.test_end_date.Year == year).ToList();

                var avg = curr.Average(ra => ra.measure_value);
                var min = curr.Min(ra => ra.measure_value);
                var max = curr.Max(ra => ra.measure_value);

                if (coll.Count() > 0)
                {
                    var model = new RadonModel()
                    {
                        type = "radon",
                        zip = zip,
                        minValue = min,
                        maxValue = max,
                        numberOfTests = (uint)curr.Count(),
                        average = Math.Round(avg, 2),

                        median = curr.OrderBy(ra => ra.measure_value)
                        .ElementAt(curr.Count() / 2)
                        .measure_value + curr.ElementAt((curr.Count() - 1) / 2)
                        .measure_value,

                        year = (uint)year,

                        year_data = coll.Where(rr => rr.test_end_date.Year >= 1990)
                        .GroupBy(rr => rr.test_end_date.Year)
                        .Select(rr => (uint)rr.Key )
                        .ToList(),

                        average_data = coll.Where(rr => rr.test_end_date.Year >= 1990)
                        .GroupBy(rr=> rr.test_end_date.Year)                        
                        .Select(rr => 
                            Math.Round(rr.Average(m => m.measure_value), 2)
                        ).ToList()
                        //averageColor
                    };

                    _radonRepo.SaveRadonModel(model);
                    return Ok(model);
                    //HttpResponseMessage response = Response.CreateResponse(HttpStatusCode.OK, model);
                }
                else
                {
                    return NotFound(new { message = "Radon not found" });
                }
            }
        }



        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}