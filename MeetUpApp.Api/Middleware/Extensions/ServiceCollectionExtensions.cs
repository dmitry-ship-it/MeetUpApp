using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;

namespace MeetUpApp.Api.Middleware.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtBearerAuthentication(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDistributedMemoryCache();
            services.AddSessionForJwtBearer();
            services.AddAuthenticationForJwtBearer()
                .AddPreconfiguredJwtBearer(configuration);

            return services;
        }

        private static IServiceCollection AddSessionForJwtBearer(
            this IServiceCollection services)
        {
            return services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.Name = "JWToken";
                options.Cookie.IsEssential = true;
            });
        }

        private static AuthenticationBuilder AddAuthenticationForJwtBearer(
            this IServiceCollection services)
        {
            return services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });
        }
    }
}
