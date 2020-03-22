using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/video/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private List<MovieDto> movieDtos = new List<MovieDto>()
            {
                new MovieDto { Title = "Test1", AltTitle = "Test1", Description = "Test1 Desc", Duration = "120", ReleaseYear = new DateTime(2000, 1, 1) },
                new MovieDto { Title = "Test2", AltTitle = "Test2", Description = "Test2 Desc", Duration = "130", ReleaseYear = new DateTime(2010, 1, 1) },
                new MovieDto { Title = "Test3", AltTitle = "Test3", Description = "Test3 Desc", Duration = "140", ReleaseYear = new DateTime(2020, 1, 1) },
                new MovieDto { Title = "Test3", AltTitle = "Test3-2", Description = "Test3 Desc", Duration = "140", ReleaseYear = new DateTime(2020, 1, 1) }
            };

        public MovieController()
        {

        }

        [HttpGet]
        public ActionResult<List<MovieDto>> GetAll()
        {
            return Ok(movieDtos);
        }

        [HttpGet("with-title/{title}")]
        public ActionResult<List<MovieDto>> GetAllWithTittle(string title)
        {
            string titleLower = title.ToLower();
            List<MovieDto> s = movieDtos.FindAll(s => s.Title.Replace(" ", "-").ToLower() == titleLower);

            if (s.Count == 0)
            {
                return NotFound();
            }

            return Ok(s);
        }

        [HttpGet("{altTitle}")]
        public ActionResult<MovieDto> Get(string altTitle)
        {
            string titleLower = altTitle.ToLower();
            MovieDto s = movieDtos.FirstOrDefault(s => s.AltTitle.Replace(" ", "-").ToLower() == titleLower);

            if (s == null)
            {
                return NotFound();
            }

            return Ok(s);
        }

        [HttpPost]
        public ActionResult Post([FromBody]MovieDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            movieDtos.Add(model);

            string key = model.AltTitle.ToLower();
            return Created($"api/video/movie/" + key, null);
        }

        [HttpPut("{altTitle}")]
        public ActionResult Put(string altTitle, [FromBody]MovieDto model)
        {
            string titleLower = altTitle.ToLower();
            MovieDto s = movieDtos.FirstOrDefault(s => s.Title.Replace(" ", "-").ToLower() == titleLower);

            if (s == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            movieDtos[movieDtos.IndexOf(s)] = model;

            return NoContent();
        }

        [HttpDelete("{altTitle}")]
        public ActionResult Delete(string altTitle)
        {
            string titleLower = altTitle.ToLower();
            MovieDto s = movieDtos.FirstOrDefault(s => s.AltTitle.Replace(" ", "-").ToLower() == titleLower);

            if (s == null)
            {
                return NotFound();
            }

            movieDtos.Remove(s);

            return NoContent();
        }
    }
}
