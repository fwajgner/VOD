using API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TypeController : Controller
    {
        // GET: api/v1/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/v1/<controller>/5
        [HttpGet("{name}")]
        public string Get(string name)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody] TypeDto type)
        {

        }

        // PUT api/v1/<controller>/5
        [HttpPut("{name}")]
        public void Put(string name, [FromBody] TypeDto type)
        {

        }

        // DELETE api/v1/<controller>/5
        [HttpDelete("{name}")]
        public void Delete(string name)
        {

        }
    }
}
