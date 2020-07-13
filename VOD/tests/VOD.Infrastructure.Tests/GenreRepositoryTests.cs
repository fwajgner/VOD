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

    public class GenreRepositoryTests : IDisposable
    {
        #region Constructors, properties

        public GenreRepositoryTests()
        {
            this.ContextFactory = new SqLiteContextFactory();
            this.RepoUnderTest = new GenreRepository(this.ContextFactory.CreateContext());
        }

        protected SqLiteContextFactory ContextFactory { get; private set; }

        protected GenreRepository RepoUnderTest { get; private set; }

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

        ~GenreRepositoryTests()
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

        public class GetAsync : GenreRepositoryTests
        {
            [Fact]
            public async Task Should_return_all_genres()
            {
                //Arrange
                GenData.RepeatCount = 3;
                List<Genre> expectedGenres = new List<Genre>(GenData.RepeatCount);
                GenData.AddManyTo(expectedGenres, TestDataFactory.CreateGenre);

                using (VODContext context = ContextFactory.CreateContext())
                {
                    context.Genres.AddRange(expectedGenres);
                    context.SaveChanges();
                }

                //Act
                IEnumerable<Genre> result = await RepoUnderTest.GetAsync();

                //Assert
                result.Should().BeEquivalentTo(expectedGenres);
            }

            [Fact]
            public async Task Should_return_the_expected_genre()
            {
                //Arrange
                Genre expectedGenre = TestDataFactory.CreateGenre();

                using (VODContext context = ContextFactory.CreateContext())
                {
                    context.Genres.Add(expectedGenre);
                    context.SaveChanges();
                }

                //Act
                Genre result = await RepoUnderTest.GetAsync(expectedGenre.Id);

                //Assert
                result.Should().BeEquivalentTo(expectedGenre);
            }

            [Fact]
            public async Task Should_return_null()
            {
                //Arrange
                Genre expectedGenre = TestDataFactory.CreateGenre();

                using (VODContext context = ContextFactory.CreateContext())
                {
                    context.Genres.Add(expectedGenre);
                    context.SaveChanges();
                }

                //Act
                Genre result = await RepoUnderTest.GetAsync(new Guid());

                //Assert
                Assert.Null(result);
            }
        }    

        public class Add : GenreRepositoryTests
        {
            [Fact]
            public void Should_create_and_return_the_specified_genre()
            {
                // Arrange
                Genre expectedGenre = TestDataFactory.CreateGenre();

                // Act
                Genre result = RepoUnderTest.Add(expectedGenre);

                // Assert
                result.Should().BeEquivalentTo(expectedGenre);
            }
        }

        public class Update : GenreRepositoryTests
        {
            [Fact]
            public void Should_update_and_return_the_specified_genre()
            {
                // Arrange
                Genre oldGenre = TestDataFactory.CreateGenre();

                using (VODContext context = ContextFactory.CreateContext())
                {
                    context.Genres.Add(oldGenre);
                    context.SaveChanges();
                }

                Genre newGenre = new Genre()
                {
                    Id = oldGenre.Id,
                    Name = "new genre",
                    Videos = null
                };

                // Act
                Genre result = RepoUnderTest.Update(newGenre);

                // Assert
                result.Should().BeEquivalentTo(newGenre);
            }
        }
    }
}
