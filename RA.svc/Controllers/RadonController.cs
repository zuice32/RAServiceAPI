using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RA.RadonRepository;
using RA.svc.Infrastructure;

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

        //TODO: refactor
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
                init = await _radonRepo.createRadonModel(_url, zip, year);

                if (init != null)
                { 
                    return Ok(init);                    
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