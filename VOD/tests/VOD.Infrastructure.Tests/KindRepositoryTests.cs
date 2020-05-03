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

    public class KindRepositoryTests
    {
        #region Constructors, properties

        public KindRepositoryTests()
        {
            this.ContextFactory = new SqLiteContextFactory();
            this.RepoUnderTest = new KindRepository(this.ContextFactory.CreateContext());
        }

        protected SqLiteContextFactory ContextFactory { get; private set; }

        protected KindRepository RepoUnderTest { get; private set; }

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

        ~KindRepositoryTests()
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

        public class GetAsync : KindRepositoryTests
        {
            [Fact]
            public async Task Should_return_all_kinds()
            {
                //Arrange
                GenData.RepeatCount = 3;
                List<Kind> expectedKinds = new List<Kind>(GenData.RepeatCount);
                GenData.AddManyTo(expectedKinds, TestDataFactory.CreateKind);

                using (VODContext context = ContextFactory.CreateContext())
                {
                    context.Kinds.AddRange(expectedKinds);
                    context.SaveChanges();
                }

                //Act
                IEnumerable<Kind> result = await RepoUnderTest.GetAsync();

                //Assert
                result.Should().BeEquivalentTo(expectedKinds);
            }

            [Fact]
            public async Task Should_return_the_expected_Kind()
            {
                //Arrange
                Kind expectedKind = TestDataFactory.CreateKind();

                using (VODContext context = ContextFactory.CreateContext())
                {
                    context.Kinds.Add(expectedKind);
                    context.SaveChanges();
                }

                //Act
                Kind result = await RepoUnderTest.GetAsync(expectedKind.Id);

                //Assert
                result.Should().BeEquivalentTo(expectedKind);
            }

            [Fact]
            public async Task Should_return_null()
            {
                //Arrange
                Kind expectedKind = TestDataFactory.CreateKind();

                using (VODContext context = ContextFactory.CreateContext())
                {
                    context.Kinds.Add(expectedKind);
                    context.SaveChanges();
                }

                //Act
                Kind result = await RepoUnderTest.GetAsync(new Guid());

                //Assert
                Assert.Null(result);
            }
        }

        public class Add : KindRepositoryTests
        {
            [Fact]
            public void Should_create_and_return_the_specified_Kind()
            {
                // Arrange
                Kind expectedKind = TestDataFactory.CreateKind();

                // Act
                Kind result = RepoUnderTest.Add(expectedKind);

                // Assert
                result.Should().BeEquivalentTo(expectedKind);
            }
        }

        public class Update : KindRepositoryTests
        {
            [Fact]
            public void Should_update_and_return_the_specified_Kind()
            {
                // Arrange
                Kind oldKind = TestDataFactory.CreateKind();

                using (VODContext context = ContextFactory.CreateContext())
                {
                    context.Kinds.Add(oldKind);
                    context.SaveChanges();
                }

                Kind newKind = new Kind()
                {
                    Id = oldKind.Id,
                    Name = "new Kind",
                    Videos = null
                };

                // Act
                Kind result = RepoUnderTest.Update(newKind);

                // Assert
                result.Should().BeEquivalentTo(newKind);
            }
        }
    }
}
