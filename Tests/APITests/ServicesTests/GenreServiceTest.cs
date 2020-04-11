namespace Tests.APITests.ServicesTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;
    using API.Services;
    using Entities;
    using Tests.Fixtures;
    using Context;
    using AutoFixture;
    using API.Exceptions;
    using System.Linq;
    using API.Services.Interfaces;
    using FluentAssertions;

    public class GenreServiceTest : IDisposable
    {
        #region Contructor, properties
        
        public GenreServiceTest()
        {
            this.ContextFactory = new SqLiteContextFactory();
            ServiceUnderTest = new GenreService(ContextFactory.CreateContext());
            TypeServiceForTest = new TypeService(ContextFactory.CreateContext());
            VideoServiceForTest = new VideoService(ContextFactory.CreateContext(), TypeServiceForTest, ServiceUnderTest);
        }  

        protected SqLiteContextFactory ContextFactory { get; private set; }

        protected GenreService ServiceUnderTest { get; private set; }

        protected IVideoService VideoServiceForTest { get; private set; }

        protected ITypeService TypeServiceForTest { get; private set; }

        protected Fixture GenData { get; set; } = new Fixture(); 
        
        public void Dispose()
        {
            ContextFactory.Dispose();
        }

        #endregion

        public class ReadAllAsync : GenreServiceTest
        {
            [Fact]
            public async Task Should_return_all_genres()
            {
                //Arrange
                List<Genre> expectedGenres = new List<Genre>();
                GenData.RepeatCount = 3;
                GenData.AddManyTo(expectedGenres, TestDataFactory.CreateGenre);

                using (ApplicationDbContext context = ContextFactory.CreateContext())
                {
                    context.Genres.AddRange(expectedGenres);
                    context.SaveChanges();
                }

                //Act
                IEnumerable<Genre> result = await ServiceUnderTest.ReadAllAsync();

                //Assert           
                result.Should().BeEquivalentTo(expectedGenres);
            }
        }

        public class ReadOneAsync : GenreServiceTest
        {
            [Fact]
            public async Task Should_return_the_expected_genre()
            {
                //Arrange
                Genre expectedGenre = TestDataFactory.CreateGenre();

                using (ApplicationDbContext context = ContextFactory.CreateContext())
                {
                    context.Genres.Add(expectedGenre);
                    context.SaveChanges();
                }

                //Act
                Genre result = await ServiceUnderTest.ReadOneAsync(expectedGenre.Name);

                //Assert
                result.Should().BeEquivalentTo(expectedGenre);
            }


            [Fact]
            public async Task Should_throw_GenreNotFoundException_when_the_genre_does_not_exist()
            {
                //Arrange
                string genreName = "unexisting genre";

                //Act, Assert
                await Assert.ThrowsAsync<GenreNotFoundException>(() => ServiceUnderTest.ReadOneAsync(genreName));               
            }
        }

        public class ReadOneWithChildrenAsync : GenreServiceTest
        {
            [Fact]
            public async Task Should_return_the_expected_genre()
            {
                //Arrange
                Genre expectedGenre = TestDataFactory.CreateGenre();
                VideoGenre videoGenre = TestDataFactory.CreateVideoGenreWithoutVideo(expectedGenre);
                Video video = TestDataFactory.CreateVideo(TestDataFactory.CreateType(), new List<VideoGenre>(){ videoGenre });

                using (ApplicationDbContext context = ContextFactory.CreateContext())
                {
                    context.Videos.Add(video);
                    context.SaveChanges();
                }

                //Act
                Genre result = await ServiceUnderTest.ReadOneWithChildrenAsync(expectedGenre.Name);

                //Assert
                result.Should().BeEquivalentTo(expectedGenre, o => o.Excluding(g => g.VideoLinks));
                result.VideoLinks.Should().BeEquivalentTo(expectedGenre.VideoLinks, 
                    o => o.Excluding(vl => vl.Video)
                    .Excluding(vl => vl.Genre));
            }


            [Fact]
            public async Task Should_throw_GenreNotFoundException_when_the_genre_does_not_exist()
            {
                //Arrange
                string genreName = "unexisting genre";

                //Act, Assert
                await Assert.ThrowsAsync<GenreNotFoundException>(() => ServiceUnderTest.ReadOneWithChildrenAsync(genreName));
            }
        }

        public class IsGenreExistsAsync : GenreServiceTest
        {           
            [Fact]
            public async Task Should_return_true_if_the_genre_exist()
            {
                // Arrange
                Genre expectedGenre = TestDataFactory.CreateGenre();

                using (ApplicationDbContext context = ContextFactory.CreateContext())
                {
                    context.Genres.Add(expectedGenre);
                    context.SaveChanges();
                }

                // Act
                bool result = await ServiceUnderTest.IsGenreExistsAsync(expectedGenre.Name);

                // Assert
                Assert.True(result);
            }

            [Fact]
            public async Task Should_return_false_if_the_genre_does_not_exist()
            {
                // Arrange
                string genreName = "unexisting genre";

                // Act
                bool result = await ServiceUnderTest.IsGenreExistsAsync(genreName);

                // Assert
                Assert.False(result);
            }
        }

        public class CreateAsync : GenreServiceTest
        {          
            [Fact]
            public async Task Should_create_and_return_the_specified_genre()
            {
                // Arrange
                Genre expectedGenre = TestDataFactory.CreateGenre();
                expectedGenre.ModificationDate = null;

                // Act
                Genre result = await ServiceUnderTest.CreateAsync(expectedGenre);
                Genre readedResult = await ServiceUnderTest.ReadOneAsync(expectedGenre.Name);

                // Assert
                result.Should().BeEquivalentTo(expectedGenre, o => o.Excluding(g => g.CreationDate));
                Assert.Equal(DateTime.Now, expectedGenre.CreationDate, TimeSpan.FromMinutes(1));
                Assert.Equal(result, readedResult);
            }
        }

        public class UpdateAsync : GenreServiceTest
        {
           [Fact]
            public async Task Should_update_and_return_the_specified_genre()
            {
                // Arrange
                Genre oldGenre = TestDataFactory.CreateGenre();
                
                using (ApplicationDbContext context = ContextFactory.CreateContext())
                {
                    context.Genres.Add(oldGenre);
                    context.SaveChanges();
                }

                Genre newGenre = TestDataFactory.CreateGenre();

                // Act
                Genre result = await ServiceUnderTest.UpdateAsync(oldGenre.Name, newGenre);
                Genre readedResult = await ServiceUnderTest.ReadOneWithChildrenAsync(newGenre.Name);

                // Assert
                await Assert.ThrowsAsync<GenreNotFoundException>(() => ServiceUnderTest.ReadOneAsync(oldGenre.Name));
                result.Should().BeEquivalentTo(newGenre, o => 
                    o.Excluding(g => g.VideoLinks)
                    .Excluding(g => g.ModificationDate)
                    .Excluding(g => g.CreationDate)
                    .Excluding(g => g.Id));
                Assert.Equal(DateTime.Now, result.ModificationDate.Value, TimeSpan.FromMinutes(1));
                Assert.Equal(result, readedResult);
            }
        }

        public class DeleteAsync : GenreServiceTest
        {
            [Fact]
            public async Task Should_delete_and_return_the_specified_genre()
            {
                // Arrange                             
                Genre createdGenre = TestDataFactory.CreateGenre();
                VideoGenre videoGenre = TestDataFactory.CreateVideoGenreWithoutVideo(createdGenre);
                Video video = TestDataFactory.CreateVideo(TestDataFactory.CreateType(), new List<VideoGenre>() { videoGenre });

                using (ApplicationDbContext context = ContextFactory.CreateContext())
                {
                    context.Videos.Add(video);
                    context.SaveChanges();
                }

                // Act
                Genre result = await ServiceUnderTest.DeleteAsync(createdGenre.Name);
                Video resultVideo = await VideoServiceForTest.ReadOneWithChildrenAsync(video.AltTitle);

                // Assert
                Assert.Equal(createdGenre.Name, result.Name);
                Assert.Equal(createdGenre.CreationDate, result.CreationDate);
                Assert.Empty(resultVideo.GenreLinks);
                await Assert.ThrowsAsync<GenreNotFoundException>(() => ServiceUnderTest.ReadOneAsync(createdGenre.Name));
            }
        }
    }
}
