using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;

namespace MeetUpApp.Api.Middleware.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtBearerAuthentication(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDistributedMemoryCache()
                .AddSession()
                .AddAuthenticationForJwtBearer()
                .AddPreconfiguredJwtBearer(configuration);

            return services;
        }

        public static IServiceCollection AddSwaggerGenWithOAuth(
            this IServiceCollection services, IConfiguration configuration)
        {
            var authSection = configuration.GetSection("AuthSettings");

            return services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new()
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new()
                    {
                        ClientCredentials = new()
                        {
                            AuthorizationUrl = new(authSection["Authority"] + "connect/authorize"),
                            TokenUrl = new(authSection["Authority"] + "connect/token"),
                            Scopes = new Dictionary<string, string>()
                            {
                                [authSection["Audience"]!] = authSection["Audience"]!
                            }
                        }
                    },
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                options.AddSecurityRequirement(new()
                {
                    {
                        new()
                        {
                            Reference = new()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
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
