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
                init = await _waterRepo.createWaterModel(_url, latitude, longitude, year);

                if (init != null)
                {
                    return Ok(init);                    
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
