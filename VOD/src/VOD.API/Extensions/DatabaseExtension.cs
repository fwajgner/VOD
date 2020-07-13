using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using VOD.Infrastructure;

namespace VOD.API.Extensions
{
    public static class DatabaseExtension
    {
        public static IServiceCollection AddVODContext(this IServiceCollection services, string connectionString)
        {
            return services
                //.AddEntityFrameworkSqlServer()
                .AddDbContext<VODContext>(opt =>
                {
                    //opt.UseSqlServer(
                    //    connectionString,
                    //    _ => 
                    //    { 
                    //        _.MigrationsAssembly(typeof(Startup)
                    //            .GetTypeInfo()
                    //            .Assembly
                    //            .GetName().Name);
                    //    });
                    opt.UseSqlServer(
                         connectionString,
                         x =>
                         {
                             x.MigrationsAssembly(typeof(Startup)
                                 .GetTypeInfo()
                                 .Assembly
                                 .GetName().Name);
                         });
                });
        }
    }
}
