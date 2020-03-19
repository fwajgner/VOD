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
        private List<SeriesDto> seriesDtos = new List<SeriesDto>()
            {
                new SeriesDto { Title = "Test1", AltTitle = "Test1s01e01", Description = "Test1 Desc", Duration = "120", ReleaseYear = new DateTime(2000, 1, 1), Episode = "1", Season = "1" },
                new SeriesDto { Title = "Test2", AltTitle = "Test2s01e02", Description = "Test2 Desc", Duration = "130", ReleaseYear = new DateTime(2010, 1, 1), Episode = "2", Season = "1" },
                new SeriesDto { Title = "Test3", AltTitle = "Test3s01e03", Description = "Test3 Desc", Duration = "140", ReleaseYear = new DateTime(2020, 1, 1), Episode = "3", Season = "1" },
                new SeriesDto { Title = "Test3", AltTitle = "Test3s02e01", Description = "Test3 Desc", Duration = "140", ReleaseYear = new DateTime(2020, 1, 1), Episode = "1", Season = "2" }
            };

        public SeriesController()
        {

        }

        [HttpGet]
        public ActionResult<List<SeriesDto>> GetAll()
        {           
            return Ok(seriesDtos);
        }

        [HttpGet("with-title/{title}")]
        public ActionResult<List<SeriesDto>> GetAllWithTittle(string title)
        {
            string titleLower = title.ToLower();
            List<SeriesDto> s = seriesDtos.FindAll(s => s.Title.Replace(" ", "-").ToLower() == titleLower);

            if (s.Count == 0)
            {
                return NotFound();
            }

            return Ok(s);
        }

        [HttpGet("{altTitle}")]
        public ActionResult<SeriesDto> Get(string altTitle)
        {
            string titleLower = altTitle.ToLower();
            SeriesDto s = seriesDtos.FirstOrDefault(s => s.AltTitle.Replace(" ", "-").ToLower() == titleLower);

            if (s == null)
            {
                return NotFound();
            }

            return Ok(s);
        }

        [HttpPost]
        public ActionResult Post([FromBody]SeriesDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            seriesDtos.Add(model);

            string key = model.AltTitle.ToLower();
            return Created($"api/video/series/" + key, null);
        }

        [HttpPut("{altTitle}")]
        public ActionResult Put(string altTitle, [FromBody]SeriesDto model)
        {
            string titleLower = altTitle.ToLower();
            SeriesDto s = seriesDtos.FirstOrDefault(s => s.AltTitle.Replace(" ", "-").ToLower() == titleLower);

            if (s == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }      
                       
            seriesDtos[seriesDtos.IndexOf(s)] = model;

            return NoContent();
        }        

        [HttpDelete("{altTitle}")]
        public ActionResult Delete(string altTitle)
        {
            string titleLower = altTitle.ToLower();
            SeriesDto s = seriesDtos.FirstOrDefault(s => s.AltTitle.Replace(" ", "-").ToLower() == titleLower);

            if (s == null)
            {
                return NotFound();
            }

            seriesDtos.Remove(s);

            return NoContent();
        }       
    }
}