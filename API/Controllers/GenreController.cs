using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GenreController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{name}")]
        public string Get(string name)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody] GenreDto genre)
        {

        }

        // PUT api/<controller>/5
        [HttpPut("{name}")]
        public void Put(string name, [FromBody] GenreDto genre)
        {

        }

        // DELETE api/<controller>/5
        [HttpDelete("{name}")]
        public void Delete(string name)
        {

        }
    }
}
