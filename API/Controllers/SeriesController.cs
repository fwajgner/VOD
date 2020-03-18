using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/video/[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        public SeriesController()
        {

        }

        [HttpGet]
        public ActionResult<List<SeriesDto>> GetAll()
        {
            List<SeriesDto> seriesDtos = new List<SeriesDto>()
            {
                new SeriesDto { Title = "Test1", Description = "Test1 Desc", Duration = 120, ReleaseYear = new DateTime(2000, 1, 1), Episode = 1, Season = 1},
                new SeriesDto { Title = "Test2", Description = "Test2 Desc", Duration = 130, ReleaseYear = new DateTime(2010, 1, 1), Episode = 2, Season = 1},
                new SeriesDto { Title = "Test3", Description = "Test3 Desc", Duration = 140, ReleaseYear = new DateTime(2020, 1, 1), Episode = 3, Season = 1}
            };
            return Ok(seriesDtos);
        }

        [HttpGet("{title}")]
        public ActionResult<List<SeriesDto>> GetAllWithTittle(string title)
        {
            SeriesDto s = new SeriesDto { Title = "Test1", Description = "Test1 Desc", Duration = 120, ReleaseYear = new DateTime(2000, 1, 1), Episode = 1, Season = 1 };
            return Ok(s);
        }

        [HttpGet("{title}/{id}")]
        public ActionResult<SeriesDto> Get(string title, string id)
        {
            SeriesDto s = new SeriesDto { Title = "Test1", Description = "Test1 Desc", Duration = 120, ReleaseYear = new DateTime(2000, 1, 1), Episode = 1, Season = 1 };
            return Ok(s);
        }

        [HttpGet("{altTitle}")]
        public ActionResult<SeriesDto> Get(string altTitle)
        {
            SeriesDto s = new SeriesDto { Title = "Test1", Description = "Test1 Desc", Duration = 120, ReleaseYear = new DateTime(2000, 1, 1), Episode = 1, Season = 1 };
            return Ok(s);
        }

        [HttpPost]
        public ActionResult Post([FromBody]SeriesDto model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            
            return Created($"api/video/series/" + "key", null);
        }

        [HttpPut("{altTitle}")]
        public ActionResult Put(string altTitle, [FromBody]SeriesDto model)
        {
            //if (meetup == null)
            //{
            //    return NotFound();
            //}

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }         

            return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult PutInId(string id, [FromBody]SeriesDto model)
        {
            //if (meetup == null)
            //{
            //    return NotFound();
            //}

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{altTitle}")]
        public ActionResult Delete(string altTitle)
        {
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteWithId(string id)
        {
            return NoContent();
        }
    }
}