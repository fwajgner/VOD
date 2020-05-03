namespace VOD.API
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Microsoft.EntityFrameworkCore;
    using VOD.Infrastructure;
    using VOD.API.Extensions;
    using VOD.Domain.Repositories;
    using VOD.Infrastructure.Repositories;
    using VOD.Domain.Extensions;
    using Polly;
    using Microsoft.Data.SqlClient;

    public class Startup
    {        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public string AngularDev { get; } = "AngularFrontDev";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddVODContext(Configuration.GetConnectionString("DockerMSSQLConnection"));
            services.AddVODContext(Configuration.GetConnectionString("LocalMSSQLConnection"));

            services.AddScoped<IKindRepository, KindRepository>()
                    .AddScoped<IGenreRepository, GenreRepository>()
                    .AddScoped<IVideoRepository, VideoRepository>();

            services.AddMappers()
                    .AddServices()
                    .AddControllers()
                    .AddValidation();


            services.AddCors(opt =>
            {
                opt.AddPolicy(AngularDev, cfg =>
                { 
                    cfg.AllowAnyOrigin();
                    //cfg.WithOrigins("http://localhost:4200");
                });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "VOD API",
                    Version = "v1",
                    Contact = new OpenApiContact()
                    {
                        Name = "Filip Wajgner",
                        Email = "204240@edu.p.lodz.pl"
                    },
                    Description = "Api documentation for VOD",
                    License = new OpenApiLicense()
                    {
                        Name = "Use under Apache License, Version 2.0",
                        Url = new Uri("http://www.apache.org/licenses/LICENSE-2.0")
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //ExecuteMigrations(app, env);
            app.UseCors(AngularDev);

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthorization();
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "VOD API v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();               
            });

            

            //using (IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            //{
            //    VODContext dbContext = serviceScope.ServiceProvider.GetService<VODContext>();
                
            //    dbContext.Database.Migrate();
            //    DbSeeder.Seed(dbContext);
            //}
        }

        private void ExecuteMigrations(IApplicationBuilder app,
         IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Testing") return;

            var retry = Policy.Handle<SqlException>()
                .WaitAndRetry(new TimeSpan[]
                {
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(6),
                    TimeSpan.FromSeconds(12)
                });

            retry.Execute(() =>
                app.ApplicationServices.GetService<VODContext>().Database.Migrate());
        }
    }
}
