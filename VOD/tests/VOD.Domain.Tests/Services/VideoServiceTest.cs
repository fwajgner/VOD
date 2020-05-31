namespace VOD.Domain.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;
    using System.Linq;
    using FluentAssertions;
    using VOD.Fixtures;
    using VOD.Domain.Services;
    using VOD.Infrastructure.Repositories;
    using MapsterMapper;
    using VOD.Domain.Responses;
    using VOD.Domain.Requests.Video;
    using AutoFixture;

    public class VideoServiceTest : IClassFixture<InMemoryContextFactory>
    {
        #region Constructors, properties

        public VideoServiceTest(InMemoryContextFactory contextFactory)
        {
            this.Repository = new VideoRepository(contextFactory.ContextInstance);
            this.Mapper = contextFactory.MapperInstance;
        }

        private VideoRepository Repository { get; set; }

        private IMapper Mapper { get; set; }

        private Fixture GenData { get; set; } = new Fixture();

        #endregion

        [Theory]
        [InlineData("6fa1bc43-ee6c-4d60-a2c8-9eb1a0494fd5", "2af1ad8c-60df-4d40-a6c7-d68da49d006a")]
        public async Task AddVideo_should_return_the_expected_video(string kindId, string genreId)
        {
            //Arrange
            string data = GenData.Create<string>();

            AddVideoRequest expectedVideo = new AddVideoRequest()
            {
                AltTitle = data,
                Description = data,
                Duration = 123,
                Episode = 7,
                ReleaseYear = DateTime.Now,
                Season = 1,
                Title = data,
                KindId = new Guid(kindId),
                GenreId = new Guid(genreId)
            };

            VideoService sut = new VideoService(Repository, Mapper);

            //Act
            VideoResponse result = await sut.AddVideoAsync(expectedVideo);

            //Assert
            result.Should().BeEquivalentTo(expectedVideo);
        }

        [Theory]
        [InlineData("2fd37626-0b1d-4d51-a243-0bc4266a7a99", "9dc0d6f2-a3d8-41a9-8393-d832c0b7a6e9")]
        public async Task EditVideo_should_return_the_expected_video(string kindId, string genreId)
        {
            //Arrange
            string data = GenData.Create<string>();

            EditVideoRequest expectedVideo = new EditVideoRequest()
            {
                Id = new Guid("eaa0B9d4-4A2d-496e-8a68-a36cd0758abb"),
                AltTitle = data,
                Description = data,
                IsInactive = false,
                Duration = 123,
                Episode = 7,
                ReleaseYear = DateTime.Now,
                Season = 1,
                Title = data,
                KindId = new Guid(kindId),
                GenreId = new Guid(genreId)
            };

            VideoService sut = new VideoService(Repository, Mapper);

            //Act
            VideoResponse result = await sut.EditVideoAsync(expectedVideo);

            //Assert
            result.Should().BeEquivalentTo(expectedVideo, o => o.Excluding(x => x.IsInactive));
        }

        [Fact]
        public async Task GetVideo_should_return_all_videos()
        {
            //Arrange
            VideoService sut = new VideoService(Repository, Mapper);

            //Act
            IEnumerable<VideoResponse> result = await sut.GetVideoAsync();

            //Assert
            result.Should().HaveCountGreaterOrEqualTo(3);
            Assert.NotNull(result.First().AltTitle);
        }

        [Theory]
        [InlineData("193d7aff-9bf5-479f-9ce5-20330846bc51")]
        [InlineData("d042d01a-1839-4558-ba94-ffc7f9237f45")]
        public async Task GetVideo_should_return_video_with_specified_id(string guid)
        {
            //Arrange
            VideoService sut = new VideoService(Repository, Mapper);

            //Act
            VideoResponse result = await sut.GetVideoAsync(new GetVideoRequest() { Id = new Guid(guid) });

            //Assert
            result.Id.Should().Be(new Guid(guid));
            Assert.NotNull(result.AltTitle);
        }

        [Fact]
        public async Task GetVideo_should_thrown_exception_with_null_id()
        {
            //Arrange
            VideoService sut = new VideoService(Repository, Mapper);

            //Act, Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetVideoAsync(null));
        }
    }
}
