namespace VOD.Infrastructure.Tests
{
    using AutoFixture;
    using FluentAssertions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using VOD.Domain.Entities;
    using VOD.Fixtures;
    using VOD.Fixtures.Data;
    using VOD.Infrastructure.Repositories;
    using Xunit;

    public class VideoRepositoryTests : IDisposable
    {
        #region Constructors, properties

        public VideoRepositoryTests()
        {
            this.ContextFactory = new SqLiteContextFactory();
            this.RepoUnderTest = new VideoRepository(this.ContextFactory.CreateContext());
        }

        protected SqLiteContextFactory ContextFactory { get; private set; }

        protected VideoRepository RepoUnderTest { get; private set; }

        protected Fixture GenData { get; set; } = new Fixture();

        #endregion

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    ContextFactory.Dispose();
                }

                this.RepoUnderTest = null;
                this.GenData = null;

                disposedValue = true;
            }
        }
        ~VideoRepositoryTests()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        public class GetAsync : VideoRepositoryTests
        {
            [Fact]
            public async Task Should_return_the_expected_video()
            {
                //Arrange
                Video expectedVideo = TestDataFactory.CreateVideo();

                using (VODContext context = ContextFactory.CreateContext())
                {
                    context.Videos.Add(expectedVideo);
                    context.SaveChanges();
                }

                //Act
                Video result = await RepoUnderTest.GetAsync(expectedVideo.Id);

                //Assert
                result.Should().BeEquivalentTo(expectedVideo, o =>
                   o.Excluding(x => x.Kind)
                    .Excluding(x => x.Genre));
            }

            [Fact]
            public async Task Should_return_all_videos()
            {
                //Arrange
                GenData.RepeatCount = 3;
                List<Video> expectedVideos = new List<Video>(GenData.RepeatCount);
                GenData.AddManyTo(expectedVideos, TestDataFactory.CreateVideo);

                using (VODContext context = ContextFactory.CreateContext())
                {
                    context.Videos.AddRange(expectedVideos);
                    context.SaveChanges();
                }

                //Act
                IEnumerable<Video> result = await RepoUnderTest.GetAsync();

                //Assert
                result.Should().BeEquivalentTo(expectedVideos, o =>
                   o.Excluding(x => x.Kind)
                    .Excluding(x => x.Genre));
            }

            [Fact]
            public async Task Should_not_return_inactive_videos()
            {
                //Arrange
                GenData.RepeatCount = 3;
                List<Video> videos = new List<Video>(GenData.RepeatCount);
                GenData.AddManyTo(videos, TestDataFactory.CreateVideo);
                videos.Last().IsInactive = true;

                using (VODContext context = ContextFactory.CreateContext())
                {
                    context.Videos.AddRange(videos);
                    context.SaveChanges();
                }

                List<Video> expectedVideos = videos.Take(2).ToList();

                //Act
                IEnumerable<Video> result = await RepoUnderTest.GetAsync();

                //Assert
                result.Any(x => x.IsInactive).Should().BeFalse();
            }

            [Fact]
            public async Task Should_return_null()
            {
                //Arrange
                Video expectedVideo = TestDataFactory.CreateVideo();

                using (VODContext context = ContextFactory.CreateContext())
                {
                    context.Videos.Add(expectedVideo);
                    context.SaveChanges();
                }

                //Act
                Video result = await RepoUnderTest.GetAsync(new Guid());

                //Assert
                Assert.Null(result);
            }
        }

        public class GetVideosContainTitleAsync : VideoRepositoryTests
        {

            [Fact]
            public async Task Should_return_videos_with_specified_title()
            {
                //Arrange
                string expectedTitle = "Expected Title";
                string titleToSearch = "EXPected";

                Video expectedVideo0 = TestDataFactory.CreateVideo();
                expectedVideo0.Title = expectedTitle;
                Video expectedVideo1 = TestDataFactory.CreateVideo();
                expectedVideo1.Title = expectedTitle;
                List<Video> videos = new List<Video>(2)
                {
                    expectedVideo0,
                    expectedVideo1
                };

                GenData.RepeatCount = 3;
                GenData.AddManyTo(videos, () => TestDataFactory.CreateVideo());

                using (VODContext context = ContextFactory.CreateContext())
                {
                    context.Videos.AddRange(videos);
                    context.SaveChanges();
                }

                List<Video> expectedVideos = videos.Take(2).ToList();

                //Act
                IEnumerable<Video> result = await RepoUnderTest.GetVideosContainTitleAsync(titleToSearch);

                //Assert
                result.Should().BeEquivalentTo(expectedVideos, o =>
                   o.Excluding(x => x.Kind)
                    .Excluding(x => x.Genre));
            }

            [Fact]
            public async Task Should_return_empty_collection()
            {
                //Arrange
                string title = "Expected Title";
                string titleToSearch = "not existing";
                Video expectedVideo0 = TestDataFactory.CreateVideo();
                expectedVideo0.Title = title;
                Video expectedVideo1 = TestDataFactory.CreateVideo();
                expectedVideo1.Title = title;
                List<Video> videos = new List<Video>(2)
                {
                    expectedVideo0,
                    expectedVideo1
                };

                using (VODContext context = ContextFactory.CreateContext())
                {
                    context.Videos.AddRange(videos);
                    context.SaveChanges();
                }

                //Act
                IEnumerable<Video> result = await RepoUnderTest.GetVideosContainTitleAsync(titleToSearch);

                //Assert
                Assert.Empty(result);
            }
        }

        public class GetVideosByGenreIdAsync : VideoRepositoryTests
        {
            [Fact]
            public async Task Should_return_the_videos_with_specified_genre()
            {
                //Arrange
                Genre expectedGenre = TestDataFactory.CreateGenre();
                Genre genre = TestDataFactory.CreateGenre();

                List<Video> videos = new List<Video>()
                {
                    TestDataFactory.CreateVideo(TestDataFactory.CreateKind(), expectedGenre),
                    TestDataFactory.CreateVideo(TestDataFactory.CreateKind(), expectedGenre),
                    TestDataFactory.CreateVideo(TestDataFactory.CreateKind(), genre)
                };

                using (VODContext context = ContextFactory.CreateContext())
                {
                    context.Videos.AddRange(videos);
                    context.SaveChanges();
                }

                List<Video> expectedVideos = videos.Take(2).ToList();

                //Act
                IEnumerable<Video> result = await RepoUnderTest.GetVideosByGenreIdAsync(expectedGenre.Id);

                //Assert
                result.Should().BeEquivalentTo(expectedVideos, o =>
                   o.Excluding(x => x.Kind)
                    .Excluding(x => x.Genre));
            }

            [Fact]
            public async Task Should_not_return_inactive_videos()
            {
                //Arrange
                Genre expectedGenre = TestDataFactory.CreateGenre();
                Genre genre = TestDataFactory.CreateGenre();

                List<Video> videos = new List<Video>()
                {
                    TestDataFactory.CreateVideo(TestDataFactory.CreateKind(), expectedGenre),
                    TestDataFactory.CreateVideo(TestDataFactory.CreateKind(), expectedGenre),
                    TestDataFactory.CreateVideo(TestDataFactory.CreateKind(), genre)
                };

                videos.First().IsInactive = true;

                using (VODContext context = ContextFactory.CreateContext())
                {
                    context.Videos.AddRange(videos);
                    context.SaveChanges();
                }

                //Act
                IEnumerable<Video> result = await RepoUnderTest.GetVideosByGenreIdAsync(expectedGenre.Id);

                //Assert
                result.Any(x => x.IsInactive).Should().BeFalse();
            }

            [Fact]
            public async Task Should_return_empty_collection()
            {
                //Arrange
                Genre notIncludedGenre = TestDataFactory.CreateGenre();

                GenData.RepeatCount = 3;
                List<Video> videos = new List<Video>(GenData.RepeatCount);
                GenData.AddManyTo(videos, TestDataFactory.CreateVideo);

                using (VODContext context = ContextFactory.CreateContext())
                {
                    context.Videos.AddRange(videos);
                    context.Genres.Add(notIncludedGenre);
                    context.SaveChanges();
                }

                //Act
                IEnumerable<Video> result = await RepoUnderTest.GetVideosByGenreIdAsync(notIncludedGenre.Id);

                //Assert
                Assert.Empty(result);
            }

            [Fact]
            public async Task Should_return_empty_collection_genre_does_not_exist()
            {
                //Arrange
                GenData.RepeatCount = 3;
                List<Video> videos = new List<Video>(GenData.RepeatCount);
                GenData.AddManyTo(videos, TestDataFactory.CreateVideo);

                using (VODContext context = ContextFactory.CreateContext())
                {
                    context.Videos.AddRange(videos);
                    context.SaveChanges();
                }

                //Act
                IEnumerable<Video> result = await RepoUnderTest.GetVideosByGenreIdAsync(Guid.NewGuid());

                //Assert
                Assert.Empty(result);
            }
        }

        public class GetVideosByKindIdAsync : VideoRepositoryTests
        {
            [Fact]
            public async Task Should_return_the_videos_with_specified_kind()
            {
                //Arrange
                Kind expectedKind = TestDataFactory.CreateKind();

                List<Video> videos = new List<Video>()
                {
                    TestDataFactory.CreateVideo(expectedKind),
                    TestDataFactory.CreateVideo(expectedKind),
                };
                GenData.RepeatCount = 1;
                GenData.AddManyTo(videos, TestDataFactory.CreateVideo);

                using (VODContext context = ContextFactory.CreateContext())
                {
                    context.Videos.AddRange(videos);
                    context.SaveChanges();
                }

                List<Video> expectedVideos = videos.Take(2).ToList();

                //Act
                IEnumerable<Video> result = await RepoUnderTest.GetVideosByKindIdAsync(expectedKind.Id);

                //Assert
                result.Should().BeEquivalentTo(expectedVideos, o =>
                   o.Excluding(x => x.Kind)
                    .Excluding(x => x.Genre));
            }

            [Fact]
            public async Task Should_not_return_inactive_videos()
            {
                //Arrange
                Kind expectedKind = TestDataFactory.CreateKind();

                List<Video> videos = new List<Video>()
                {
                    TestDataFactory.CreateVideo(expectedKind),
                    TestDataFactory.CreateVideo(expectedKind),
                };
                GenData.RepeatCount = 1;
                GenData.AddManyTo(videos, TestDataFactory.CreateVideo);
                videos.First().IsInactive = true;

                using (VODContext context = ContextFactory.CreateContext())
                {
                    context.Videos.AddRange(videos);
                    context.SaveChanges();
                }

                //Act
                IEnumerable<Video> result = await RepoUnderTest.GetVideosByKindIdAsync(expectedKind.Id);

                //Assert
                result.Any(x => x.IsInactive).Should().BeFalse();
            }

            [Fact]
            public async Task Should_return_empty_collection()
            {
                //Arrange
                Kind kindWithoutVideos = TestDataFactory.CreateKind();
                Kind kind = TestDataFactory.CreateKind();
                List<Video> videos = new List<Video>()
                {
                    TestDataFactory.CreateVideo(kind),
                    TestDataFactory.CreateVideo(kind)
                };

                using (VODContext context = ContextFactory.CreateContext())
                {
                    context.Videos.AddRange(videos);
                    context.Kinds.Add(kindWithoutVideos);
                    context.SaveChanges();
                }

                //Act
                IEnumerable<Video> result = await RepoUnderTest.GetVideosByKindIdAsync(kindWithoutVideos.Id);

                //Assert
                Assert.Empty(result);
            }

            [Fact]
            public async Task Should_return_empty_kind_does_not_exist()
            {
                //Arrange
                GenData.RepeatCount = 3;
                List<Video> expectedVideos = new List<Video>(GenData.RepeatCount);
                GenData.AddManyTo(expectedVideos, TestDataFactory.CreateVideo);

                using (VODContext context = ContextFactory.CreateContext())
                {
                    context.Videos.AddRange(expectedVideos);
                    context.SaveChanges();
                }

                //Act
                IEnumerable<Video> result = await RepoUnderTest.GetVideosByKindIdAsync(Guid.NewGuid());

                //Assert
                Assert.Empty(result);
            }
        }

        public class Add : VideoRepositoryTests
        {
            [Fact]
            public void Should_add_and_return_the_specified_video()
            {
                // Arrange
                Video expectedVideo = TestDataFactory.CreateVideo();

                //Act
                Video result = RepoUnderTest.Add(expectedVideo);

                //Assert 
                result.Should().BeEquivalentTo(expectedVideo);
            }
        }

        public class Update : VideoRepositoryTests
        {
            [Fact]
            public void Should_update_and_return_the_specified_video()
            {
                // Arrange
                Kind newKind = TestDataFactory.CreateKind();
                Genre newGenre = TestDataFactory.CreateGenre();
                Video oldVideo = TestDataFactory.CreateVideo();

                using (VODContext context = ContextFactory.CreateContext())
                {
                    context.Videos.Add(oldVideo);
                    context.Genres.Add(newGenre);
                    context.Kinds.Add(newKind);
                    context.SaveChanges();
                }

                Video newVideo = TestDataFactory.CreateVideo(newKind, newGenre);
                newVideo.Id = oldVideo.Id;

                //Act
                Video result = RepoUnderTest.Update(newVideo);

                //Assert 
                result.Should().BeEquivalentTo(newVideo);
            }

            [Fact]
            public void Should_update_and_add_new_properties_then_return_the_specified_video()
            {
                // Arrange              
                Video oldVideo = TestDataFactory.CreateVideo();

                using (VODContext context = ContextFactory.CreateContext())
                {
                    context.Videos.Add(oldVideo);
                    context.SaveChanges();
                }

                Kind newKind = TestDataFactory.CreateKind();
                Genre newGenre = TestDataFactory.CreateGenre();
                Video newVideo = TestDataFactory.CreateVideo(newKind, newGenre);
                newVideo.Id = oldVideo.Id;

                //Act
                Video result = RepoUnderTest.Update(newVideo);

                //Assert 
                result.Should().BeEquivalentTo(newVideo);
            }
        }
    }
}
