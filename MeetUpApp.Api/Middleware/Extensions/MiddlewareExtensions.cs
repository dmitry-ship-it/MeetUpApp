using FluentValidation;
using FluentValidation.AspNetCore;
using MeetUpApp.Data;
using MeetUpApp.Managers;
using MeetUpApp.ViewModels.Validation;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Data;
using System.Text;

namespace MeetUpApp.Api.Middleware.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IHostBuilder UsePreconfiguredSerilog(this IHostBuilder builder)
        {
            Serilog.ILogger logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            return builder.UseSerilog(logger);
        }

        public static IApplicationBuilder TryAddFirstUser(this IApplicationBuilder builder)
        {
            using var services = builder.ApplicationServices.CreateScope();
            var db = services.ServiceProvider.GetRequiredService<AppDataContext>();
            if (!db.User.Any())
            {
                var userManager = services.ServiceProvider.GetRequiredService<UserManager>();
                userManager.AddUser("admin", "Qs3PGVAyyhUXtkRw").Wait();
            }

            return builder;
        }

        public static IServiceCollection AddSessionForJwtBearer(this IServiceCollection services)
        {
            return services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.Name = "JWToken";
                options.Cookie.IsEssential = true;
            });
        }

        public static AuthenticationBuilder AddAuthenticationForJwtBearer(
            this IServiceCollection services)
        {
            return services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });
        }

        public static AuthenticationBuilder AddPreconfiguredJwtBearer(
            this AuthenticationBuilder builder,
            IConfiguration configuration)
        {
            var key = Encoding.Default.GetBytes(configuration
                .GetRequiredSection("AuthSettings")["Token"]!);

            return builder.AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    //ValidIssuer = jwtConfigs["ApiDomain"]!,
                    ValidateAudience = false,
                    //ValidAudience = jwtConfigs["ApiDomain"]!,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        }

        public static IServiceCollection AddPreconfiguredFluentValidation(
            this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<UserModelValidator>();
            services.AddFluentValidationRulesToSwagger();

            return services;
        }

        public static IApplicationBuilder UseJwtBearer(this IApplicationBuilder app)
        {
            return app.Use(async (context, next) =>
            {
                var token = context.Session.GetString("JWToken");
                if (!string.IsNullOrEmpty(token))
                {
                    context.Request.Headers.Add("Authorization", $"Bearer {token}");
                }

                await next();
            });
        }
    }
}
