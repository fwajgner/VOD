namespace Tests.APITests.ServicesTests
{
    using API.Services;
    using AutoFixture;
    using Context;
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Tests.Fixtures;
    using Xunit;
    using API.Services.Interfaces;
    using System.Linq;
    using API.Exceptions;
    using FluentAssertions;

    public class VideoServiceTest : IDisposable
    {
        #region Constructors, properties

        public VideoServiceTest()
        {
            this.ContextFactory = new SqLiteContextFactory();
            this.TypeServiceForTest = new TypeService(ContextFactory.CreateContext());
            this.GenreServiceForTest = new GenreService(ContextFactory.CreateContext());            
            this.ServiceUnderTest = new VideoService(ContextFactory.CreateContext(), TypeServiceForTest, GenreServiceForTest);
        }

        protected SqLiteContextFactory ContextFactory { get; private set; }

        protected VideoService ServiceUnderTest { get; private set; }

        protected ITypeService TypeServiceForTest { get; private set; }    

        protected IGenreService GenreServiceForTest { get; private set; }

        protected Fixture GenData { get; set; } = new Fixture();

        public void Dispose()
        {
            ContextFactory.Dispose();
        }

        #endregion

        public class ReadOneAsync : VideoServiceTest
        {            
            [Fact]
            public async Task Should_return_the_expected_video()
            {
                //Arrange
                Video expectedVideo = TestDataFactory.CreateVideo();

                using (ApplicationDbContext context = ContextFactory.CreateContext())
                {
                    context.Videos.Add(expectedVideo);
                    context.SaveChanges();
                }

                //Act
                Video result = await ServiceUnderTest.ReadOneAsync(expectedVideo.AltTitle);

                //Assert
                result.Should().BeEquivalentTo(expectedVideo, o => o.Excluding(v => v.Type).Excluding(v => v.GenreLinks));
                Assert.Null(result.Type);
                Assert.Null(result.GenreLinks);
            }


            [Fact]
            public async Task Should_throw_VideoNotFoundException_when_the_video_does_not_exist()
            {
                //Arrange
                string altTitle = "alt0";

                //Act, Assert
                await Assert.ThrowsAsync<VideoNotFoundException>(() => ServiceUnderTest.ReadOneAsync(altTitle));
            }
        }

        public class ReadOneWithChildrenAsync : VideoServiceTest
        {
            [Fact]
            public async Task Should_return_the_expected_video()
            {
                //Arrange
                Video expectedVideo = TestDataFactory.CreateVideo();

                using (ApplicationDbContext context = ContextFactory.CreateContext())
                {
                    context.Videos.Add(expectedVideo);
                    context.SaveChanges();
                }

                //Act
                Video result = await ServiceUnderTest.ReadOneWithChildrenAsync(expectedVideo.AltTitle);

                //Assert               
                result.Should().BeEquivalentTo(expectedVideo, o => o.Excluding(v => v.Type).Excluding(v => v.GenreLinks));
                result.GenreLinks.Should().BeEquivalentTo(expectedVideo.GenreLinks, o =>
                    o.Excluding(vg => vg.Video).Excluding((vg => vg.Genre)));
                result.Type.Should().BeEquivalentTo(expectedVideo.Type, o => o.Excluding(t => t.Videos));
            }


            [Fact]
            public async Task Should_throw_VideoNotFoundException_when_the_video_does_not_exist()
            {
                //Arrange
                string altTitle = "alt0";

                //Act, Assert
                await Assert.ThrowsAsync<VideoNotFoundException>(() => ServiceUnderTest.ReadOneWithChildrenAsync(altTitle));
            }
        }

        public class ReadAllContainTitleAsync : VideoServiceTest
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
                List<Video> videosWithExpectedTitle = new List<Video>(2)
                {
                    expectedVideo0,
                    expectedVideo1
                };
                List<Video> videos = new List<Video>(videosWithExpectedTitle);
                GenData.RepeatCount = 3;
                GenData.AddManyTo(videos, () => TestDataFactory.CreateVideo());

                using (ApplicationDbContext context = ContextFactory.CreateContext())
                {
                    context.Videos.AddRange(videos);
                    context.SaveChanges();
                }

                //Act
                IEnumerable<Video> result = await ServiceUnderTest.ReadAllContainTitleAsync(titleToSearch);

                //Assert
                Assert.Equal(2, result.Count());
                result.Should().BeEquivalentTo(videosWithExpectedTitle, o =>
                    o.Excluding(v => v.GenreLinks)
                    .Excluding(v => v.Type.Videos));
            }

            [Fact]
            public async Task Should_return_null()
            {
                //Arrange
                string expectedTitle = "Expected Title";
                string titleToSearch = "not existing";
                Video expectedVideo0 = TestDataFactory.CreateVideo();
                expectedVideo0.Title = expectedTitle;
                Video expectedVideo1 = TestDataFactory.CreateVideo();
                expectedVideo1.Title = expectedTitle;
                List<Video> videosWithExpectedTitle = new List<Video>(2)
                {
                    expectedVideo0,
                    expectedVideo1
                };
                List<Video> videos = new List<Video>(videosWithExpectedTitle);

                using (ApplicationDbContext context = ContextFactory.CreateContext())
                {
                    context.Videos.AddRange(videos);
                    context.SaveChanges();
                }

                //Act
                IEnumerable<Video> result = await ServiceUnderTest.ReadAllContainTitleAsync(titleToSearch);

                //Assert
                Assert.Null(result);
            }

            [Fact]
            public async Task Should_throw_TitleTooShortException()
            {
                //Arrange
                string titleToSearch = "12";              

                //Act, Assert
                await Assert.ThrowsAsync<TitleTooShortException>(() => ServiceUnderTest.ReadAllContainTitleAsync(titleToSearch));
            }
        }

        public class ReadAllWithGenreAsync : VideoServiceTest
        {

            [Fact]
            public async Task Should_return_the_videos_with_specified_genre()
            {
                //Arrange
                Genre expectedGenre = TestDataFactory.CreateGenre();
                Genre genre = TestDataFactory.CreateGenre();
                VideoGenre vg0 = TestDataFactory.CreateVideoGenreWithoutVideo(expectedGenre);
                VideoGenre vg1 = TestDataFactory.CreateVideoGenreWithoutVideo(expectedGenre);
                VideoGenre vg2 = TestDataFactory.CreateVideoGenreWithoutVideo(expectedGenre);
                VideoGenre vg3 = TestDataFactory.CreateVideoGenreWithoutVideo(genre);
                VideoGenre vg4 = TestDataFactory.CreateVideoGenreWithoutVideo(genre);
                List<Video> videosWithExpectedGenre = new List<Video>(3)
                {
                    TestDataFactory.CreateVideo(TestDataFactory.CreateType(), new List<VideoGenre>(){ vg0 }),
                    TestDataFactory.CreateVideo(TestDataFactory.CreateType(), new List<VideoGenre>(){ vg1, vg3 }),
                    TestDataFactory.CreateVideo(TestDataFactory.CreateType(), new List<VideoGenre>(){ vg2, vg4 }),
                };
                List<Video> videos = new List<Video>(videosWithExpectedGenre);
                GenData.RepeatCount = 2;
                GenData.AddManyTo(videos, () => TestDataFactory.CreateVideo());

                using (ApplicationDbContext context = ContextFactory.CreateContext())
                {
                    context.Videos.AddRange(videos);
                    context.SaveChanges();
                }

                //Act
                IEnumerable<Video> result = await ServiceUnderTest.ReadAllWithGenreAsync(expectedGenre.Name);

                //Assert
                Assert.Equal(3, result.Count());
                result.Should().BeEquivalentTo(videosWithExpectedGenre, o =>
                    o.Excluding(v => v.GenreLinks)
                    .Excluding(v => v.Type));
            }

            [Fact]
            public async Task Should_return_null()
            {
                //Arrange
                Genre genre0 = TestDataFactory.CreateGenre();
                Genre genre1 = TestDataFactory.CreateGenre();
                Genre notIncludedGenre = TestDataFactory.CreateGenre();
                VideoGenre vg0 = TestDataFactory.CreateVideoGenreWithoutVideo(genre0);
                VideoGenre vg1 = TestDataFactory.CreateVideoGenreWithoutVideo(genre0);
                VideoGenre vg2 = TestDataFactory.CreateVideoGenreWithoutVideo(genre1);
                List<Video> videosWithExpectedGenre = new List<Video>(3)
                {
                    TestDataFactory.CreateVideo(TestDataFactory.CreateType(), new List<VideoGenre>(){ vg0 }),
                    TestDataFactory.CreateVideo(TestDataFactory.CreateType(), new List<VideoGenre>(){ vg1, vg2 }),
                };

                using (ApplicationDbContext context = ContextFactory.CreateContext())
                {
                    context.Videos.AddRange(videosWithExpectedGenre);
                    context.Genres.Add(notIncludedGenre);
                    context.SaveChanges();
                }

                //Act
                IEnumerable<Video> result = await ServiceUnderTest.ReadAllWithGenreAsync(notIncludedGenre.Name);

                //Assert
                Assert.Null(result);
            }

            [Fact]
            public async Task Should_throw_GenreNotFoundException()
            {
                //Arrange
                string unexistingGenreName = "unexisting";

                //Act, Assert
                await Assert.ThrowsAsync<GenreNotFoundException>(() => ServiceUnderTest.ReadAllWithGenreAsync(unexistingGenreName));
            }
        }

        public class ReadAllWithTypeAsync : VideoServiceTest
        {

            [Fact]
            public async Task Should_return_the_videos_with_specified_type()
            {
                //Arrange
                Entities.Type expectedType = TestDataFactory.CreateType();
                Entities.Type type = TestDataFactory.CreateType();
                List<Video> videosWithExpectedType = new List<Video>(3) 
                {
                    TestDataFactory.CreateVideo(expectedType),
                    TestDataFactory.CreateVideo(expectedType),
                    TestDataFactory.CreateVideo(expectedType)
                };
                List<Video> videos = new List<Video>(videosWithExpectedType);
                GenData.RepeatCount = 2;
                GenData.AddManyTo(videos, () => TestDataFactory.CreateVideo(type));

                using (ApplicationDbContext context = ContextFactory.CreateContext())
                {
                    context.Videos.AddRange(videos);
                    context.SaveChanges();
                }

                //Act
                IEnumerable<Video> result = await ServiceUnderTest.ReadAllWithTypeAsync(expectedType.Name);

                //Assert
                Assert.Equal(3, result.Count());
                result.Should().BeEquivalentTo(videosWithExpectedType, o => 
                    o.Excluding(v => v.GenreLinks)
                    .Excluding(v => v.Type.Videos));
            }

            [Fact]
            public async Task Should_return_null()
            {
                //Arrange
                Entities.Type expectedType = TestDataFactory.CreateType();
                Entities.Type type = TestDataFactory.CreateType();
                List<Video> videos = new List<Video>(3);
                GenData.RepeatCount = 3;
                GenData.AddManyTo(videos, () => TestDataFactory.CreateVideo(type));

                using (ApplicationDbContext context = ContextFactory.CreateContext())
                {
                    context.Videos.AddRange(videos);
                    context.Types.Add(expectedType);
                    context.SaveChanges();
                }

                //Act
                IEnumerable<Video> result = await ServiceUnderTest.ReadAllWithTypeAsync(expectedType.Name);

                //Assert
                Assert.Null(result);
            }

            [Fact]
            public async Task Should_throw_TypeNotFoundException()
            {
                //Arrange
                string unexistingTypeName = "unexisting";

                //Act, Assert
                await Assert.ThrowsAsync<TypeNotFoundException>(() => ServiceUnderTest.ReadAllWithTypeAsync(unexistingTypeName));
            }
        }

        public class CreateAsync : VideoServiceTest
        {
            [Fact]
            public async Task Should_create_and_return_the_specified_video()
            {
                // Arrange
                Video expectedVideo = TestDataFactory.CreateVideo();

                //Act
                Video result = await ServiceUnderTest.CreateAsync(expectedVideo);
                Video readedResult = await ServiceUnderTest.ReadOneWithChildrenAsync(expectedVideo.AltTitle);

                //Assert 
                Assert.Equal(expectedVideo, result);
                Assert.Equal(DateTime.Now, expectedVideo.CreationDate, TimeSpan.FromMinutes(1));
                Assert.Equal(result, readedResult);
            }
        }

        public class UpdateAsync : VideoServiceTest
        {
            [Fact]
            public async Task Should_update_and_return_the_specified_video()
            {                
                // Arrange
                Entities.Type oldType = TestDataFactory.CreateType();
                Entities.Type newType = TestDataFactory.CreateType();

                Genre genre0 = TestDataFactory.CreateGenre();
                Genre genre1 = TestDataFactory.CreateGenre();
                Genre genre2 = TestDataFactory.CreateGenre();

                VideoGenre oldVideoGenre = TestDataFactory.CreateVideoGenreWithoutVideo(genre0);
                Video oldVideo = TestDataFactory.CreateVideo(oldType, new List<VideoGenre>(){ oldVideoGenre });                        

                using (ApplicationDbContext context = ContextFactory.CreateContext())
                {
                    context.Videos.Add(oldVideo);
                    context.Genres.Add(genre1);
                    context.Genres.Add(genre2);
                    context.SaveChanges();
                }

                string altTitle = "expected video";
                Video newVideo = new Video()
                {
                    AltTitle = altTitle,
                    Description = GenData.Create<string>(),
                    Title = altTitle,
                    Type = newType
                };
                VideoGenre newVideoGenre0 = TestDataFactory.CreateVideoGenre(newVideo, genre0);
                VideoGenre newVideoGenre1 = TestDataFactory.CreateVideoGenre(newVideo, genre1);
                VideoGenre newVideoGenre2 = TestDataFactory.CreateVideoGenre(newVideo, genre2);
                newVideo.GenreLinks = new List<VideoGenre>() { /*newVideoGenre0,*/ newVideoGenre1, newVideoGenre2 };

                //Act
                Video result = await ServiceUnderTest.UpdateAsync(oldVideo.AltTitle, newVideo);
                Video readedResult = await ServiceUnderTest.ReadOneWithChildrenAsync(altTitle);

                //Assert 
                result.Should().BeEquivalentTo(newVideo, 
                    o => o.Excluding(v => v.Id)
                    .Excluding(v => v.CreationDate)
                    .Excluding(v => v.ModificationDate)
                    .Excluding(v => v.TypeId));
                Assert.Equal(DateTime.Now, result.ModificationDate.Value, TimeSpan.FromMinutes(1));
                Assert.Equal(result, readedResult);
            }
        }

        public class DeleteAsync : VideoServiceTest
        {
            [Fact]
            public async Task Should_delete_and_return_the_specified_video()
            {
                // Arrange
                Genre genre = TestDataFactory.CreateGenre();
                VideoGenre videoGenre = TestDataFactory.CreateVideoGenreWithoutVideo(genre);
                Video expectedVideo = TestDataFactory.CreateVideo(TestDataFactory.CreateType(), new List<VideoGenre>(){ videoGenre });

                using (ApplicationDbContext context = ContextFactory.CreateContext())
                {
                    context.Videos.Add(expectedVideo);
                    context.SaveChanges();
                }

                //Act
                Video result = await ServiceUnderTest.DeleteAsync(expectedVideo.AltTitle);
                Genre genreResult = await GenreServiceForTest.ReadOneWithChildrenAsync(genre.Name);

                //Assert 
                result.Should().BeEquivalentTo(expectedVideo, o => o.Excluding(v => v.Type).Excluding(v => v.GenreLinks));
                result.GenreLinks.Should().BeEquivalentTo(expectedVideo.GenreLinks, o => 
                    o.Excluding(vg => vg.Video).Excluding((vg => vg.Genre)));
                Assert.Empty(genreResult.VideoLinks);
                await Assert.ThrowsAsync<VideoNotFoundException>(() => ServiceUnderTest.ReadOneAsync(expectedVideo.AltTitle));
            }
        }
    }
}
