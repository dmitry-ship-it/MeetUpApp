using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;

namespace MeetUpApp.Api.Middleware.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtBearerAuthentication(
            this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDistributedMemoryCache()
                .AddSession()
                .AddAuthenticationForJwtBearer()
                .AddPreconfiguredJwtBearer(configuration);

            return services;
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
