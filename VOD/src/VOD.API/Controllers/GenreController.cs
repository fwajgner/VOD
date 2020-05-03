using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VOD.API.Filters;
using VOD.Domain.Requests.Genre;
using VOD.Domain.Responses;
using VOD.Domain.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VOD.API.Controllers
{
    [Route("api/v1/genres")]
    [ApiController]
    [JsonException]
    public class GenreController : ControllerBase
    {
        public GenreController(IGenreService genreService)
        {
            this.GenreService = genreService;
        }

        private IGenreService GenreService { get; }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pageSize = 30, [FromQuery] int pageIndex = 0)
        {
            IEnumerable<GenreResponse> result = await GenreService.GetGenreAsync();

            int totalItems = result.ToList().Count;

            IEnumerable<GenreResponse> genresOnPage = result
                .OrderBy(x => x.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize);

            PaginatedResponseModel<GenreResponse> model = new PaginatedResponseModel<GenreResponse>
                (pageIndex, pageSize, totalItems, genresOnPage);

            return Ok(model);
        }

        [HttpGet("{id:guid}")]
        [GenreExists]
        public async Task<IActionResult> GetById(Guid id)
        {
            GenreResponse result = await GenreService.GetGenreAsync(new GetGenreRequest() { Id = id });

            return Ok(result);
        }

        [HttpGet("{id:guid}/videos")]
        [GenreExists]
        public async Task<IActionResult> GetVideosByGenreId(Guid id)
        {
            IEnumerable<VideoResponse> result = await GenreService.GetVideosByGenreIdAsync(new GetGenreRequest() { Id = id });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddGenreRequest request)
        {
            GenreResponse result = await GenreService.AddGenreAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = result.GenreId }, null);
        }
    }
}
