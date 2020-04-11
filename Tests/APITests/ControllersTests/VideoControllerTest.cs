namespace Tests.APITests.ControllersTests
{
    using API.Controllers;
    using API.Services.Interfaces;
    using Entities;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class VideoControllerTest : BaseHttpTest
    {
        public VideoControllerTest()
        {
            VideoServiceMock = new Mock<IVideoService>();
            ControllerUnderTest = new VideoController(VideoServiceMock.Object);
        }

        protected VideoController ControllerUnderTest { get; }

        protected Mock<IVideoService> VideoServiceMock { get; }

        protected override void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton(Genres);
        }

        public class ReadAllAsync : VideoControllerTest
        {
            [Fact]
            public async Task Should_return_OkObjectResult_with_videos()
            {
                //Arrange
                Video[] expectedVideos = new Video[]
                {
                new Video { AltTitle = "alt0", Title = "title0"},
                new Video { AltTitle = "alt1", Title = "title1"},
                new Video { AltTitle = "alt2", Title = "title2"}
                };

                //Act
                var result = await ControllerUnderTest.ReadAllAsync();

                //Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Same(expectedVideos, okResult.Value);
            }
        }    
    }
}
