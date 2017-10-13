using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RA.microservice.Model;

namespace RA.Controllers
{
    [Route("api/[controller]")]
    public class RadonController : Controller
    {
        // GET api/values
        [HttpGet]
        public object Get()
        {
            return new RadonDO() { type = "radon", zip = "17101", average = 4, limitMin = 4, limitMax = 20 };
        }

        // GET api/values/5
        [HttpGet("{zip}")]
        public string Get(string zip)
        {
            return "value";
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
