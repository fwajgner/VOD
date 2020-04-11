namespace API
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpsPolicy;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Swashbuckle.AspNetCore.Swagger;
    using Microsoft.OpenApi.Models;
    using Context;
    using Microsoft.EntityFrameworkCore;
    using Entities;
    using API.Services;
    using API.Services.Interfaces;

    public class Startup
    {        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private string AngularFrontDev { get; } = "AngularFrontDev";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("MigrationsLib"))
                );

            services.AddCors(options =>
            {
                options.AddPolicy(AngularFrontDev,
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200");
                });
            });

            services.TryAddScoped<IVideoService, VideoService>();
            services.TryAddScoped<IGenreService, GenreService>();
            services.TryAddScoped<ITypeService, TypeService>();
            services.TryAddScoped<IUserService, UserService>();

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
                app.UseCors(AngularFrontDev);
            }
         
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
            
            using (IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                ApplicationDbContext dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                
                dbContext.Database.Migrate();
                DbSeeder.Seed(dbContext);
            }
        }
    }
}
