using MeetUpApp.Data;
using MeetUpApp.Api.Managers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Data.Common;
using System.Data;
using Microsoft.EntityFrameworkCore;
using MeetUpApp.Api.ViewModels;

namespace MeetUpApp.Api
{
    public static class ProgramExtensions
    {
        public static IServiceProvider TryAddFirstUser(this IServiceProvider serviceProvider)
        {
            using var services = serviceProvider.CreateScope();
            var db = services.ServiceProvider.GetRequiredService<AppDataContext>();
            if (!db.User.Any())
            {
                var userManager = services.ServiceProvider.GetRequiredService<UserManager>();
                userManager.AddUser("admin", "Qs3PGVAyyhUXtkRw").Wait();
            }

            return serviceProvider;
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
            IConfigurationSection jwtSection)
        {
            var key = Encoding.Default.GetBytes(jwtSection["Token"]!);

            return builder.AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    //ValidIssuer = jwtConfigs["WebSiteDomain"]!,
                    ValidateAudience = false,
                    //ValidAudience = jwtConfigs["WebSiteDomain"]!,
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

        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            return app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (ArgumentException ex)
                {
                    await context.WriteExceptionMessage(
                        StatusCodes.Status400BadRequest,
                        ex.Message);
                }
                catch (DbUpdateException ex)
                {
                    await context.WriteExceptionMessage(
                        StatusCodes.Status500InternalServerError,
                        ex.Message);
                }
                catch (DbException ex)
                {
                    await context.WriteExceptionMessage(
                        StatusCodes.Status500InternalServerError,
                        ex.Message);
                }
                catch (Exception)
                {
                    await context.WriteExceptionMessage(
                        StatusCodes.Status500InternalServerError,
                        "Internal server error");
                }
            });
        }

        private static async Task WriteExceptionMessage(
            this HttpContext httpContext,
            int statusCode,
            string message)
        {
            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(new MessageModel
            {
                Message = message
            });
        }
    }
}
