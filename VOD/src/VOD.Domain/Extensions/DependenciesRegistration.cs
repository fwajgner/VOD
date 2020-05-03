namespace VOD.Domain.Extensions
{
    using System.Reflection;
    using VOD.Domain.Mappers;
    using VOD.Domain.Services;
    using FluentValidation.AspNetCore;
    using Microsoft.Extensions.DependencyInjection;
    using MapsterMapper;

    public static class DependenciesRegistration
    {
        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            services.AddSingleton(MapperConfig.CreateMap());
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<IKindService, KindService>()
                .AddScoped<IGenreService, GenreService>()
                .AddScoped<IVideoService, VideoService>();

            return services;
        }

        public static IMvcBuilder AddValidation(this IMvcBuilder builder)
        {
            builder
                .AddFluentValidation(configuration =>
                    configuration.RegisterValidatorsFromAssembly
                        (Assembly.GetExecutingAssembly()));

            return builder;
        }
    }
}
