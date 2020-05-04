namespace VOD.Infrastructure.Extensions
{
    using System.Text;
    using VOD.Domain.Configurations;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using VOD.Domain.Entities;
    using Microsoft.AspNetCore.Identity;

    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddTokenAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            IConfiguration settings = configuration.GetSection("AuthenticationSettings");
            AuthenticationSettings settingsTyped = settings.Get<AuthenticationSettings>();

            services.Configure<AuthenticationSettings>(settings);
            byte[] key = Encoding.ASCII.GetBytes(settingsTyped.Secret);

            services.AddIdentity<User, IdentityRole>()
             .AddEntityFrameworkStores<VODContext>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                        //ValidateIssuer = true,
                        //ValidateAudience = true,
                        //ValidIssuer = "yourhostname",
                        //ValidAudience = "yourhostname"
                    };
                });

            return services;
        }
    }
}
