﻿namespace VOD.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using VOD.API.Filters;
    using VOD.Domain.Requests.Video;
    using VOD.Domain.Responses;
    using VOD.Domain.Services;

    [Authorize]
    [Route("api/v1/videos")]
    [ApiController]
    [JsonException]
    public class VideoController : ControllerBase
    {
        public VideoController(IVideoService videoService)
        {
            this.VideoService = videoService ?? throw new ArgumentNullException(nameof(videoService));
        }

        private IVideoService VideoService { get; }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pageSize = 30, [FromQuery] int pageIndex = 0)
        {
            IEnumerable<VideoResponse> result = await VideoService.GetVideoAsync();

            int totalItems = result.Count();

            IEnumerable<VideoResponse> videosOnPage = result
                .OrderBy(x => x.Title)
                .Skip(pageSize * pageIndex)
                .Take(pageSize);

            PaginatedResponseModel<VideoResponse> model = new PaginatedResponseModel<VideoResponse>
                (pageIndex, pageSize, totalItems, videosOnPage);

            return Ok(model);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        [VideoExists]
        public async Task<IActionResult> GetById(Guid id)
        {
            VideoResponse result = await VideoService.GetVideoAsync(new GetVideoRequest() { Id = id });

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}/media")]
        public FileResult GetMediaById(Guid id)
        {
            string directory = "Media";
            //string relativePath = Path.Combine(directory, altTitle);
            int index = new Random().Next(2);
            string relativePath = Path.Combine(directory, $"testVideo{index}.mp4");
            string absolutePath = Path.GetFullPath(relativePath);

            return PhysicalFile(absolutePath, "application/octet-stream", enableRangeProcessing: true);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Post(AddVideoRequest request)
        {
            VideoResponse result = await VideoService.AddVideoAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, null);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("{id:guid}")]
        [VideoExists]
        public async Task<IActionResult> Put(Guid id, EditVideoRequest request)
        {
            request.Id = id;
            VideoResponse result = await VideoService.EditVideoAsync(request);

            return Ok(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id:guid}")]
        [VideoExists]
        public async Task<IActionResult> Delete(Guid id)
        {
            DeleteVideoRequest request = new DeleteVideoRequest() { Id = id };

            await VideoService.DeleteVideoAsync(request);

            return NoContent();
        }
    }
}