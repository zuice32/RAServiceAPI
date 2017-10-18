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

        // GET api/values
        [NoCache]
        [HttpGet]
        public object Get()
        {
            return new RAappDO() { type = "radon", zip = "17101", average = 4, limitMin = 4, limitMax = 20 };
        }

        // GET api/values/5
        [NoCache]
        [HttpGet("{zip}")]
        public RAappDO Get(string zip)
        {
            List<RadonRecord> coll = _radonRepo.GetAllRadonRecords().ToList().Where(rr => rr.address_postal_code == zip
            && rr.test_start_date?.Year == DateTime.Now.Year && rr.test_end_date?.Year == DateTime.Now.Year ).ToList();

            return new RAappDO()
            {
                type = "radon",
                zip = zip,
                numberOfTests = (uint)coll.Count(),
                average = coll.Average(ra => ra.measure_value.HasValue ? (double)ra.measure_value : 0.0),
                limitMin = 4,
                limitMax = 20
            };            
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
