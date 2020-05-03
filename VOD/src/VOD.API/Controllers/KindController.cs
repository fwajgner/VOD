namespace VOD.API.Controllers
{
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using VOD.API.Filters;
    using VOD.Domain.Requests.Kind;
    using VOD.Domain.Responses;
    using VOD.Domain.Services;

    [Route("api/v1/kinds")]
    [ApiController]
    [JsonException]
    public class KindController : ControllerBase
    {
        public KindController(IKindService kindService)
        {
            this.KindService = kindService;
        }

        private IKindService KindService { get; }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pageSize = 30, [FromQuery] int pageIndex = 0)
        {
            IEnumerable<KindResponse> result = await KindService.GetKindAsync();

            int totalItems = result.ToList().Count;

            IEnumerable<KindResponse> kindsOnPage = result
                .OrderBy(x => x.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize);

            PaginatedResponseModel<KindResponse> model = new PaginatedResponseModel<KindResponse>
                (pageIndex, pageSize, totalItems, kindsOnPage);

            return Ok(model);
        }

        [HttpGet("{id:guid}")]
        [KindExists]
        public async Task<IActionResult> GetById(Guid id)
        {
            KindResponse result = await KindService.GetKindAsync(new GetKindRequest() { Id = id });

            return Ok(result);
        }

        [HttpGet("{id:guid}/videos")]
        [KindExists]
        public async Task<IActionResult> GetVideosByKindId(Guid id)
        {
            IEnumerable<VideoResponse> result = await KindService.GetVideosByKindIdAsync(new GetKindRequest() { Id = id });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddKindRequest request)
        {
            KindResponse result = await KindService.AddKindAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = result.KindId }, null);
        }
    }
}
