namespace Tests.APITests.ServicesTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using API.Exceptions;
    using API.Services;
    using AutoFixture;
    using Context;
    using FluentAssertions;
    using Tests.Fixtures;
    using Xunit;

    public class TypeServiceTest : IDisposable
    {
        #region Constructors, properties
        
        public TypeServiceTest()
        {
            this.ContextFactory = new SqLiteContextFactory();
            ServiceUnderTest = new TypeService(ContextFactory.CreateContext());
        }

        protected SqLiteContextFactory ContextFactory { get; private set; }

        protected TypeService ServiceUnderTest { get; private set; }

        protected Fixture GenData { get; set; } = new Fixture();

        public void Dispose()
        {
            ContextFactory.Dispose();
        }

        #endregion

        public class ReadAllAsync : TypeServiceTest
        {
            [Fact]
            public async Task Should_return_all_types()
            {
                //Arrange
                List<Entities.Type> expectedTypes = new List<Entities.Type>();
                GenData.RepeatCount = 3;
                GenData.AddManyTo(expectedTypes, TestDataFactory.CreateType);

                using (ApplicationDbContext context = ContextFactory.CreateContext())
                {
                    context.Types.AddRange(expectedTypes);
                    context.SaveChanges();
                }

                //Act
                IEnumerable<Entities.Type> result = await ServiceUnderTest.ReadAllAsync();

                //Assert           
                result.Should().BeEquivalentTo(expectedTypes, o => o.Excluding(g => g.Videos));
            }
        }

        public class ReadOneAsync : TypeServiceTest
        {

            [Fact]
            public async Task Should_return_the_expected_type()
            {
                //Arrange
                Entities.Type expectedType = TestDataFactory.CreateType();

                using (ApplicationDbContext context = ContextFactory.CreateContext())
                {
                    context.Types.Add(expectedType);
                    context.SaveChanges();
                }

                //Act
                Entities.Type result = await ServiceUnderTest.ReadOneAsync(expectedType.Name);

                //Assert
                result.Should().BeEquivalentTo(expectedType, o => o.Excluding(g => g.Videos));
            }


            [Fact]
            public async Task Should_throw_TypeNotFoundException_when_the_type_does_not_exist()
            {
                //Arrange
                string typeName = "unexisting type";

                //Act, Assert
                await Assert.ThrowsAsync<TypeNotFoundException>(() => ServiceUnderTest.ReadOneAsync(typeName));
            }
        }

        public class IsTypeExistsAsync : TypeServiceTest
        {
            [Fact]
            public async Task Should_return_true_if_the_type_exist()
            {
                // Arrange
                Entities.Type expectedType = TestDataFactory.CreateType();

                using (ApplicationDbContext context = ContextFactory.CreateContext())
                {
                    context.Types.Add(expectedType);
                    context.SaveChanges();
                }

                // Act
                bool result = await ServiceUnderTest.IsTypeExistsAsync(expectedType.Name);

                // Assert
                Assert.True(result);
            }

            [Fact]
            public async Task Should_return_false_if_the_type_does_not_exist()
            {
                // Arrange
                string typeName = "unexisting type";

                // Act
                var result = await ServiceUnderTest.IsTypeExistsAsync(typeName);

                // Assert
                Assert.False(result);
            }
        }

        public class CreateAsync : TypeServiceTest
        {
            [Fact]
            public async Task Should_create_and_return_the_specified_type()
            {
                // Arrange
                Entities.Type expectedType = TestDataFactory.CreateType();
                expectedType.ModificationDate = null;

                // Act
                Entities.Type result = await ServiceUnderTest.CreateAsync(expectedType);
                Entities.Type readedResult = await ServiceUnderTest.ReadOneAsync(expectedType.Name);

                // Assert
                result.Should().BeEquivalentTo(expectedType, o => o.Excluding(g => g.Videos));
                Assert.Equal(DateTime.Now, result.CreationDate, TimeSpan.FromMinutes(1));
                Assert.Null(result.ModificationDate);
                Assert.Equal(result, readedResult);
            }
        }

        public class UpdateAsync : TypeServiceTest
        {
            [Fact]
            public async Task Should_update_and_return_the_specified_type()
            {
                // Arrange
                Entities.Type oldType = TestDataFactory.CreateType();              

                using (ApplicationDbContext context = ContextFactory.CreateContext())
                {
                    context.Types.Add(oldType);
                    context.SaveChanges();
                }

                Entities.Type newType = TestDataFactory.CreateType();

                // Act
                Entities.Type result = await ServiceUnderTest.UpdateAsync(oldType.Name, newType);
                Entities.Type readedResult = await ServiceUnderTest.ReadOneAsync(newType.Name);

                // Assert
                await Assert.ThrowsAsync<TypeNotFoundException>(() => ServiceUnderTest.ReadOneAsync(oldType.Name));
                result.Should().BeEquivalentTo(newType, o =>
                    o.Excluding(g => g.Videos)
                    .Excluding(g => g.ModificationDate)
                    .Excluding(g => g.CreationDate)
                    .Excluding(g => g.Id));
                Assert.Equal(DateTime.Now, result.ModificationDate.Value, TimeSpan.FromMinutes(1));
                Assert.Equal(result, readedResult);    
            }
        }

        public class DeleteAsync : TypeServiceTest
        {
            [Fact]
            public async Task Should_delete_and_return_the_specified_type()
            {
                // Arrange
                Entities.Type createdType = TestDataFactory.CreateType();

                using (ApplicationDbContext context = ContextFactory.CreateContext())
                {
                    context.Types.Add(createdType);
                    context.SaveChanges();
                }

                // Act
                Entities.Type result = await ServiceUnderTest.DeleteAsync(createdType.Name);

                // Assert
                result.Should().BeEquivalentTo(createdType, o => o.Excluding(t => t.Videos));
                await Assert.ThrowsAsync<TypeNotFoundException>(() => ServiceUnderTest.ReadOneAsync(createdType.Name));
            }
        }
    }
}
