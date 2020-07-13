namespace VOD.Fixtures
{
    using MapsterMapper;
    using Microsoft.EntityFrameworkCore;
    using System;
    using VOD.Fixtures.Data;
    using VOD.Infrastructure;

    public class InMemoryContextFactory
    {
        public InMemoryContextFactory()
        {
            DbContextOptions<VODContext> contextOptions = new 
                DbContextOptionsBuilder<VODContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            this.EnsureCreation(contextOptions);

            ContextInstance = new TestVODContext(contextOptions);
        }

        public TestVODContext ContextInstance { get; }

        public IMapper MapperInstance { get; } = new Mapper(TestDataFactory.CreateMapper());

        private void EnsureCreation(DbContextOptions<VODContext> contextOptions)
        {
            using var context = new TestVODContext(contextOptions);
            context.Database.EnsureCreated();
        }
    }
}
