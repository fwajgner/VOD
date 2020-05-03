namespace VOD.API.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using VOD.API.Exceptions;
    using VOD.Domain.Requests.Video;
    using VOD.Domain.Responses;
    using VOD.Domain.Services;

    public class VideoExistsAttribute : TypeFilterAttribute
    {
        public VideoExistsAttribute() : base(typeof(VideoExistsFilterImpl))
        {

        }

        public class VideoExistsFilterImpl : IAsyncActionFilter
        {
            public VideoExistsFilterImpl(IVideoService videoService)
            {
                this.VidService = videoService;
            }

            private IVideoService VidService { get; }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (!(context.ActionArguments["id"] is Guid id))
                {
                    context.Result = new BadRequestResult();
                    return;
                }

                VideoResponse result = await VidService.GetVideoAsync(new GetVideoRequest { Id = id });

                if (result == null)
                {
                    context.Result = new NotFoundObjectResult(new JsonErrorPayload
                    { DetailedMessage = $"Video with id {id} not exist." });
                    return;
                }

                await next();
            }
        }
    }
}
