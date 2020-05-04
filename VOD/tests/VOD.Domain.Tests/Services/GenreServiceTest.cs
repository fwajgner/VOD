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
    using VOD.Domain.Requests.Genre;
    using AutoFixture;

    public class GenreServiceTest : IClassFixture<InMemoryContextFactory>
    {
        #region Constructors, properties

        public GenreServiceTest(InMemoryContextFactory contextFactory)
        {
            this.GenreRepo = new GenreRepository(contextFactory.ContextInstance);
            this.VideoRepo = new VideoRepository(contextFactory.ContextInstance);
            this.Mapper = contextFactory.MapperInstance;
        }

        private GenreRepository GenreRepo { get; set; }

        private VideoRepository VideoRepo { get; set; }

        private IMapper Mapper { get; set; }

        private Fixture GenData { get; set; } = new Fixture();

        #endregion

        [Fact]
        public async Task AddGenre_should_return_the_expected_genre()
        {
            //Arrange
            AddGenreRequest expectedGenre = new AddGenreRequest()
            {
                GenreName = GenData.Create<string>()
            };

            GenreService sut = new GenreService(GenreRepo, VideoRepo, Mapper);

            //Act
            GenreResponse result = await sut.AddGenreAsync(expectedGenre);

            //Assert
            //expectedGenre.Should().BeEquivalentTo(result, o =>
            //    o.Excluding(x => x.GenreId));
            Assert.Equal(expectedGenre.GenreName, result.Name);
        }      

        [Fact]
        public async Task GetGenre_should_return_all_Genres()
        {
            //Arrange
            GenreService sut = new GenreService(GenreRepo, VideoRepo, Mapper);

            //Act
            IEnumerable<GenreResponse> result = await sut.GetGenreAsync();

            //Assert
            result.Should().HaveCountGreaterOrEqualTo(2);
            Assert.NotNull(result.First().Name);
        }

        [Theory]
        [InlineData("2af1ad8c-60df-4d40-a6c7-d68da49d006a")]
        public async Task GetGenre_should_return_genre_with_specified_id(string guid)
        {
            //Arrange
            GenreService sut = new GenreService(GenreRepo, VideoRepo, Mapper);

            //Act
            GenreResponse result = await sut.GetGenreAsync(new GetGenreRequest() { Id = new Guid(guid) });

            //Assert
            result.GenreId.Should().Be(new Guid(guid));
            Assert.NotNull(result.Name);
        }

        [Fact]
        public async Task GetGenre_should_thrown_exception_with_null_id()
        {
            //Arrange
            GenreService sut = new GenreService(GenreRepo, VideoRepo, Mapper);

            //Act, Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetGenreAsync(null));
        }

        [Theory]
        [InlineData("9dc0d6f2-a3d8-41a9-8393-d832c0b7a6e9")]
        public async Task GetVideosByGenreId_should_return_specified_videos(string guid)
        {
            //Arrange
            GenreService sut = new GenreService(GenreRepo, VideoRepo, Mapper);

            //Act
            IEnumerable<VideoResponse> result = await sut.GetVideosByGenreIdAsync(new GetGenreRequest() { Id = new Guid(guid) });

            //Assert
            Assert.Equal(2, result.Count());
            Assert.NotNull(result.First().AltTitle);
        }

        [Fact]
        public async Task GetVideosByGenreId_should_thrown_exception_with_null_id()
        {
            //Arrange
            GenreService sut = new GenreService(GenreRepo, VideoRepo, Mapper);

            //Act, Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetVideosByGenreIdAsync(null));
        }
    }
}
