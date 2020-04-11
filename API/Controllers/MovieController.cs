using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Context;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mapster;
using Entities;
using API.Services;

namespace API.Controllers
{
    [Route("api/v1/video/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        public MovieController(ApplicationDbContext context)
        {
            DbContext = context;
        }

        private ApplicationDbContext DbContext { get; set; }

        [HttpGet]
        public ActionResult<List<MovieDto>> GetAll([FromQuery] string title)
        {
            List<MovieDto> moviesList = DbContext.Videos
                .Include(v => v.GenreLinks)
                    .ThenInclude(vg => vg.Genre)
                .AsNoTracking()
                .ProjectToType<MovieDto>()
                .ToList();

            if (title != null && title.Length >= 3)
            {
                string titleLower = title.ToLower();
                return Ok(moviesList.Where(v => v.Title.Replace(" ", "-").ToLower() == titleLower));                    
            }

            return Ok(moviesList);
        }

        [HttpGet("with-title/{title}")]
        public ActionResult<List<MovieDto>> GetAllWithTittle(string title)
        {
            string titleLower = title.ToLower();
            List<MovieDto> moviesList = DbContext.Videos
                .Where(v => v.Title.Replace(" ", "-").ToLower() == titleLower)
                .AsNoTracking()
                .ProjectToType<MovieDto>()
                .ToList();

            if (moviesList.Count == 0)
            {
                return NotFound(new 
                { 
                    Error = $"Not found any movies with title: {title}!"    
                });
            }

            return Ok(moviesList);
        }

        [HttpGet("{altTitle}")]
        public ActionResult<MovieDto> Get([FromRoute] string altTitle)
        {
            string titleLower = altTitle.ToLower();
            MovieDto movie = DbContext.Videos
                .Where(v => v.AltTitle.Replace(" ", "-").ToLower() == titleLower)
                .AsNoTracking()
                .ProjectToType<MovieDto>()
                .FirstOrDefault();

            if (movie == null)
            {
                return NotFound(new
                {
                    Error = $"Not found any movies with the alternative title: {altTitle}!"
                });
            }

            return Ok(movie);
        }

        [HttpPost]
        public ActionResult Post([FromBody] MovieDto model)
        {
            if (model == null)
            {
                return new StatusCodeResult(500);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!ushort.TryParse(model.Duration, out ushort duration))
            {
                return new StatusCodeResult(500);
            }

            // get type id from db
            string typeNameLower = model.TypeName.ToLower();
            Entities.Type type = DbContext.Types
                .Where(t => t.Name.ToLower() == typeNameLower)
                .AsNoTracking()
                .FirstOrDefault();

            if (type == null)
            {
                return NotFound(new
                {
                    Error = $"Not found any types of video like: {model.TypeName}!"
                });
            }

            // get genres identifiers from db and add them to list
            List<int> genresId = new List<int>();

            foreach (GenreDto genre in model.Genres)
            {
                string genreNameLower = genre.Name.ToLower();
                Genre genreFromDb = DbContext.Genres
               .Where(g => g.Name.ToLower() == genreNameLower)
               .AsNoTracking()
               .FirstOrDefault();

                if (genreFromDb == null)
                {
                    return NotFound(new
                    {
                        Error = $"Not found any genre of video like: {genre.Name.ToLower()}!"
                    });
                }

                // adding genre id to list
                genresId.Add(genreFromDb.Id);
            }      

            // creating new video object
            Video movie = new Video()
            {
                AltTitle = model.AltTitle,
                CreationDate = DateTime.Now,
                Description = model.Description,
                Duration = duration,
                ReleaseYear = model.ReleaseYear,
                Title = model.Title,
                TypeId = type.Id,
            };

            // adding video to db and link video with genres
            foreach (int id in genresId)
            {
                VideoGenre videoGenre = new VideoGenre()
                {
                    GenreId = id,
                    Video = movie
                };

                DbContext.VideosGenres.Add(videoGenre);
            }

            DbContext.SaveChanges();

            return Created($"api/video/movie/{movie.AltTitle.Replace(" ", "-").ToLower()}", null);
        }

        [HttpPut("{altTitle}")]
        public ActionResult Put([FromRoute] string altTitle, [FromBody] MovieDto model)
        {
            if (model == null)
            {
                return new StatusCodeResult(500);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!ushort.TryParse(model.Duration, out ushort duration))
            {
                return new StatusCodeResult(500);
            }

            // get video from db
            string titleLower = altTitle.ToLower();
            Video movie = DbContext.Videos
                .Where(v => v.AltTitle.Replace(" ", "-").ToLower() == titleLower)
                .Include(v => v.GenreLinks)
                    .ThenInclude(vg => vg.GenreId)
                .FirstOrDefault();

            if (movie == null)
            {
                return NotFound(new
                {
                    Error = $"Not found any movies with the alternative title: {altTitle}!"
                });
            }

            // get type of video from db
            string typeNameLower = model.TypeName.ToLower();
            Entities.Type type = DbContext.Types
                .Where(t => t.Name.ToLower() == typeNameLower)
                .AsNoTracking()
                .FirstOrDefault();

            if (type == null)
            {
                return NotFound(new
                {
                    Error = $"Not found any types of video like: {model.TypeName}!"
                });
            }

            // get genres identifiers from db and add them to list
            List<int> genresId = new List<int>();

            foreach (GenreDto genre in model.Genres)
            {
                string genreNameLower = genre.Name.ToLower();
                Genre genreFromDb = DbContext.Genres
               .Where(g => g.Name.ToLower() == genreNameLower)
               .AsNoTracking()
               .FirstOrDefault();

                if (genreFromDb == null)
                {
                    return NotFound(new
                    {
                        Error = $"Not found any genre of video like: {genre.Name.ToLower()}!"
                    });
                }

                // adding genre id to list
                genresId.Add(genreFromDb.Id);
            }

            // modification of video
            movie.AltTitle = model.AltTitle;
            movie.Description = model.Description;
            movie.Duration = duration;
            movie.ReleaseYear = model.ReleaseYear;
            movie.Title = model.Title;
            movie.TypeId = type.Id;

            movie.ModificationDate = DateTime.Now;

            DbContext.SaveChanges();
                    
            return NoContent();
        }

        [HttpDelete("{altTitle}")]
        public ActionResult Delete([FromRoute] string altTitle)
        {
            string titleLower = altTitle.ToLower();
            Video movie = DbContext.Videos
                .Where(v => v.AltTitle.Replace(" ", "-").ToLower() == titleLower)
                .FirstOrDefault();

            if (movie == null)
            {
                return NotFound(new
                {
                    Error = $"Not found any movies with the alternative title: {altTitle}!"
                });
            }

            DbContext.Videos.Remove(movie);
            DbContext.SaveChanges();

            return NoContent();
        }
    }
}
