namespace Tests.APITests
{
    using API;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Net.Http;

    public abstract class BaseHttpTest : IDisposable
    {
        public BaseHttpTest()
        {
            IWebHostBuilder builder = new WebHostBuilder()
                .UseEnvironment(Environment)
                .ConfigureServices(ConfigureServices)
                .UseStartup<Startup>();

            Server = new TestServer(builder);
            Client = Server.CreateClient();
            Client.BaseAddress = BaseAddress;
        }

        protected TestServer Server { get; }

        protected HttpClient Client { get; }

        protected virtual Uri BaseAddress => new Uri("http://localhost/api/");

        protected virtual string Environment =>  "Development";

        protected virtual void ConfigureServices(IServiceCollection services)
        {

        }

        #region IDisposable

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Client.Dispose();
                    Server.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
