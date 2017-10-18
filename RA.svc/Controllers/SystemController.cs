using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RA.RadonRepository;
using RA.DALAccess;
using Newtonsoft.Json;
using RA.MongoDB;

namespace RA.microservice.Controllers
{
    [Route("api/[controller]")]
    public class SystemController : Controller
    {
        private readonly IRadonRepo _radonRepo;

        public SystemController(IRadonRepo radonRepo)
        {
            _radonRepo = radonRepo;
        }

        // Call an initialization - api/system/init
        [HttpGet("{setting}")]
        public async Task<string> Get(string setting)
        {
            if (setting == "init")
            {
                //if empty fill with data from site
                if (!_radonRepo.GetAllRadonRecords().Any())
                {
                    using (Client<RadonRecord> client = new Client<RadonRecord>("http://data.pa.gov/resource/"))
                    {
                        if (await client.CheckConnection())
                        {
                            //string json = await client.Get("x7jf-72k4.json?$limit=1000000");
                            List<RadonRecord> result = client.Get("7ypj-ezu6.json?$limit=2400000").ToList();
                            //JsonConvert.DeserializeObject<List<RadonRecord>>(json);

                            _radonRepo.SaveRadonRecords(result);
                            return "populated";
                        }
                    }
                }                
            }

            return "Unknown";
        }
    }
}
