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
    using VOD.Domain.Requests.Kind;
    using AutoFixture;

    public class KindServiceTest : IClassFixture<InMemoryContextFactory>
    {
        #region Constructors, properties

        public KindServiceTest(InMemoryContextFactory contextFactory)
        {
            this.KindRepo = new KindRepository(contextFactory.ContextInstance);
            this.VideoRepo = new VideoRepository(contextFactory.ContextInstance);
            this.Mapper = contextFactory.MapperInstance;
        }

        private KindRepository KindRepo { get; set; }

        private VideoRepository VideoRepo { get; set; }

        private IMapper Mapper { get; set; }

        private Fixture GenData { get; set; } = new Fixture();

        #endregion

        [Fact]
        public async Task AddKind_should_return_the_expected_kind()
        {
            //Arrange
            AddKindRequest expectedKind = new AddKindRequest()
            {
                KindName = GenData.Create<string>()
            };

            KindService sut = new KindService(KindRepo, VideoRepo, Mapper);

            //Act
            KindResponse result = await sut.AddKindAsync(expectedKind);

            //Assert
            //expectedKind.Should().BeEquivalentTo(result, o =>
            //    o.Excluding(x => x.KindId));
            Assert.Equal(expectedKind.KindName, result.Name);
        }

        [Fact]
        public async Task GetKind_should_return_all_Kinds()
        {
            //Arrange
            KindService sut = new KindService(KindRepo, VideoRepo, Mapper);

            //Act
            IEnumerable<KindResponse> result = await sut.GetKindAsync();

            //Assert
            result.Should().HaveCountGreaterOrEqualTo(2);
            Assert.NotNull(result.First().Name);
        }

        [Theory]
        [InlineData("6fa1bc43-ee6c-4d60-a2c8-9eb1a0494fd5")]
        public async Task GetKind_should_return_kind_with_specified_id(string guid)
        {
            //Arrange
            KindService sut = new KindService(KindRepo, VideoRepo, Mapper);

            //Act
            KindResponse result = await sut.GetKindAsync(new GetKindRequest() { Id = new Guid(guid) });

            //Assert
            result.KindId.Should().Be(new Guid(guid));
            Assert.NotNull(result.Name);
        }

        [Fact]
        public async Task GetKind_should_thrown_exception_with_null_id()
        {
            //Arrange
            KindService sut = new KindService(KindRepo, VideoRepo, Mapper);

            //Act, Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetKindAsync(null));
        }


        [Theory]
        [InlineData("2fd37626-0b1d-4d51-a243-0bc4266a7a99")]
        public async Task GetVideosByKindId_should_return_specified_videos(string guid)
        {
            //Arrange
            KindService sut = new KindService(KindRepo, VideoRepo, Mapper);

            //Act
            IEnumerable<VideoResponse> result = await sut.GetVideosByKindIdAsync(new GetKindRequest() { Id = new Guid(guid) });

            //Assert
            Assert.Equal(2, result.Count());
            Assert.NotNull(result.First().AltTitle);
        }

        [Fact]
        public async Task GetVideosByKindId_should_thrown_exception_with_null_id()
        {
            //Arrange
            KindService sut = new KindService(KindRepo, VideoRepo, Mapper);

            //Act, Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetVideosByKindIdAsync(null));
        }
    }
}
