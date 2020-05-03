namespace VOD.API.Tests.Filters
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Routing;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using VOD.API.Filters;
    using VOD.Domain.Requests.Video;
    using VOD.Domain.Responses;
    using VOD.Domain.Services;
    using Xunit;

    public class VideoExistsAttributeTests
    {
        [Fact]
        public async Task Should_continue_pipeline_when_id_is_present()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            Mock<IVideoService> videoService = new Mock<IVideoService>();

            videoService
                .Setup(x => x.GetVideoAsync(It.IsAny<GetVideoRequest>()))
                .ReturnsAsync(new VideoResponse { Id = id });

            var filter = new VideoExistsAttribute.VideoExistsFilterImpl(videoService.Object);

            ActionExecutingContext actionExecutedContext = new ActionExecutingContext(
                new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()),
                new List<IFilterMetadata>(),
                new Dictionary<string, object>
                {
                    {"id", id}
                }, new object());

            Mock<ActionExecutionDelegate> nextCallback = new Mock<ActionExecutionDelegate>();

            //Act
            await filter.OnActionExecutionAsync(actionExecutedContext, nextCallback.Object);

            //Assert
            nextCallback.Verify(executionDelegate => executionDelegate(), Times.Once);
        }

        [Fact]
        public async Task Should_not_continue_pipeline_cuz_id_is_not_present()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            Mock<IVideoService> videoService = new Mock<IVideoService>();

            videoService
                .Setup(x => x.GetVideoAsync(It.IsAny<GetVideoRequest>()))
                .ReturnsAsync(() => null);

            var filter = new VideoExistsAttribute.VideoExistsFilterImpl(videoService.Object);

            ActionExecutingContext actionExecutedContext = new ActionExecutingContext(
                new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()),
                new List<IFilterMetadata>(),
                new Dictionary<string, object>
                {
                    {"id", id}
                }, new object());

            Mock<ActionExecutionDelegate> nextCallback = new Mock<ActionExecutionDelegate>();

            //Act
            await filter.OnActionExecutionAsync(actionExecutedContext, nextCallback.Object);

            //Assert
            nextCallback.Verify(executionDelegate => executionDelegate(), Times.Never);
        }
    }
}
