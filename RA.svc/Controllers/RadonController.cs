using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RA.microservice.Model;
using RA.RadonRepository;
using RA.microservice.Infrastructure;
using System.Collections;
using RA.DALAccess;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Web;

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

        private readonly string _url = "http://data.pa.gov/resource/";

        // GET api/values
        [NoCache]
        [HttpGet]
        public object Get()
        {
            return new RAappDO() { type = "radon", zip = "17101", average = 4 };
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
                    coll = client.Get("7ypj-ezu6.json?address_postal_code=" + zip).ToList();
                }
            }

            coll = coll.Where(rr => rr.test_start_date.Year == year || rr.test_end_date.Year == year).ToList();
            var avg = coll.Average(ra => ra.measure_value);

            if (coll.Count() > 0)
            {
                var model = new RAappDO()
                {
                    type = "radon",
                    zip = zip,
                    numberOfTests = (uint)coll.Count(),
                    average = (double)avg,
                    maxYear = (uint)year,
                    minYear = (uint)year
                };

                return Ok(model);
                //HttpResponseMessage response = Response.CreateResponse(HttpStatusCode.OK, model);
            }
            else
            {
                return NotFound();
                //return new HttpResponseMessage(HttpStatusCode.NotFound);
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
                    coll = client.Get("7ypj-ezu6.json?address_postal_code=" + zip).ToList();
                }
            }

            if (coll.Count() > 0)
            {
                return Ok(new RAappDO()
                {
                    type = "radon",
                    zip = zip,
                    numberOfTests = (uint)coll.Count(),
                    average = (double)coll.Average(ra => ra.measure_value),

                });
            }
            else {
                return NotFound();
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