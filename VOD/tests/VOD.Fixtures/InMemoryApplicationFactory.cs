namespace VOD.Fixtures
{
    using System;
    using VOD.Infrastructure;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public class InMemoryApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseEnvironment("Testing")
                .ConfigureTestServices(services =>
                {
                    DbContextOptions<VODContext> options = new
                     DbContextOptionsBuilder<VODContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString())
                        .Options;

                    services.AddScoped<VODContext>(serviceProvider => new TestVODContext(options));

                    ServiceProvider sp = services.BuildServiceProvider();

                    using IServiceScope scope = sp.CreateScope();

                    IServiceProvider scopedServices = scope.ServiceProvider;
                    VODContext db = scopedServices.GetRequiredService<VODContext>();
                    db.Database.EnsureCreated();
                });
        }
    }
}
