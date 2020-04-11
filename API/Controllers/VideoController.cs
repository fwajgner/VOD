using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Services.Interfaces;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class VideoController : Controller
    {
        public VideoController(IVideoService videoService)
        {
            this.VideoService = videoService ?? throw new ArgumentNullException(nameof(videoService));
        }

        private IVideoService VideoService { get; }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Video>), 200)]
        public async Task<IActionResult> ReadAllAsync()
        {
            //IEnumerable<Video> allVideos = await VideoService.ReadAllAsync();
            return Ok();
        }
    }
}