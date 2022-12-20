using MeetUpApp.Data;
using MeetUpApp.Managers;
using MeetUpApp.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Data;
using System.Data.Common;
using System.Text;

namespace MeetUpApp.Api.CustomMiddlewares
{
    public static class MiddlewareExtensions
    {
        public static IHostBuilder UsePreconfiguredSerilog(this IHostBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            return builder.UseSerilog(Log.Logger);
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

        public static IApplicationBuilder UsePreconfiguredExceptionHandler(
            this IApplicationBuilder builder)
        {
            return builder.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception ex)
                {
                    await WriteExceptionMessage(context.Response, ex);
                    LogException(ex, Log.Logger);
                }
            });
        }

        private static async Task WriteExceptionMessage(
            this HttpResponse respose,
            Exception ex)
        {
            respose.StatusCode =
                ex is ArgumentException or DbException or DbUpdateException
                    ? StatusCodes.Status400BadRequest
                    : StatusCodes.Status500InternalServerError;

            await respose.WriteAsJsonAsync(new MessageModel
            {
                Message = GetErrorMessage(ex)
            });
        }

        private static void LogException(Exception ex, Serilog.ILogger logger)
        {
            logger.Warning(GetErrorMessage(ex));
        }

        private static string GetErrorMessage(Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}