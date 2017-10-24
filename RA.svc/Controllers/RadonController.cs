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

        // GET api/values
        [NoCache]
        [HttpGet]
        public object Get()
        {
            return new RadonModel() { };
        }

        [NoCache]
        [HttpGet("{zip}~{yearStart}~{yearEnd}")]
        public async Task<IActionResult> GetRadonAverageYearRange(string zip, int yearStart, int yearEnd)
        {
            //List<RadonRecord> coll = _radonRepo.GetAllRadonRecords().ToList(); //.ToList().Where(rr => rr.address_postal_code == zip)

            List<RadonRecord> coll = new List<RadonRecord>();
            using (Client<RadonRecord> client = new Client<RadonRecord>(_url))
            {
                if (await client.CheckConnection())
                {
                    coll = client.Get("?address_postal_code=" + zip).ToList();
                }
            }

            coll = coll.Where(rr => (rr.test_start_date.Year >= yearStart || rr.test_start_date.Year <= yearEnd)
            && (rr.test_end_date.Year >= yearStart || rr.test_end_date.Year <= yearEnd)).ToList();
            var avg = coll.Average(ra => ra.measure_value);

            if (coll.Count() > 0)
            {
                var model = new RadonModel()
                {
                    type = "radon",
                    zip = zip,
                    numberOfTests = (uint)coll.Count(),
                    average = (double)avg,
                    maxYear = (uint)yearEnd,
                    minYear = (uint)yearStart,
                    //averageColor
                };
                //_radonRepo.SaveRadonModel(model);
                return Ok(model);
                //HttpResponseMessage response = Response.CreateResponse(HttpStatusCode.OK, model);
            }
            else
            {
                return NotFound(new { message = "Radon not found" });
            }
        }

        // GET api/values/5
        [NoCache]
        [HttpGet("{zip}~{year}")]
        public async Task<IActionResult> GetRadonAverageYear(string zip, int year)
        {
            //were getting bamboozled with requirements and mongodb isn't working as it does locally. Probably needs indexing/networking/config changes yadadadad.
            //Grabbing records based on the queries designed beforehand and populating the list entities with them.
            //List<RadonRecord> coll = _radonRepo.GetAllRadonRecords().ToList(); //.ToList().Where(rr => rr.address_postal_code == zip)
            
            List<RadonRecord> coll = new List<RadonRecord>();
            using (Client<RadonRecord> client = new Client<RadonRecord>(_url))
            {
                if (await client.CheckConnection())
                {
                    coll = client.Get("?address_postal_code=" + zip).ToList();
                }
            }

            coll = coll.Where(rr => rr.test_start_date.Year == year || rr.test_end_date.Year == year).ToList();
            var avg = coll.Average(ra => ra.measure_value);

            if (coll.Count() > 0)
            {
                var model = new RadonModel()
                {
                    type = "radon",
                    zip = zip,
                    numberOfTests = (uint)coll.Count(),
                    average = (double)avg,
                    maxYear = (uint)year,
                    minYear = (uint)year,
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

        [NoCache]
        [HttpGet("{zip}")]
        public async Task<IActionResult> GetRadonAverage(string zip)
        {
            List<RadonRecord> coll = new List<RadonRecord>();
            using (Client<RadonRecord> client = new Client<RadonRecord>(_url))
            {
                if (await client.CheckConnection())
                {
                    coll = client.Get("?address_postal_code=" + zip).ToList();
                }
            }

            if (coll.Count() > 0)
            {
                var model = new RadonModel()
                {
                    type = "radon",
                    zip = zip,
                    numberOfTests = (uint)coll.Count(),
                    average = (double)coll.Average(ra => ra.measure_value),

                };
                _radonRepo.SaveRadonModel(model);
                return Ok(model);
            }
            else {
                return NotFound(new { message = "Radon not found" });
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