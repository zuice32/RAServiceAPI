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
                    using (Client client = new Client("http://data.pa.gov/resource/"))
                    {
                        if (await client.CheckConnection())
                        {
                            string json = await client.Get("7ypj-ezu6.json");
                            List<RadonRecord> result = JsonConvert.DeserializeObject<List<RadonRecord>>(json);
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
