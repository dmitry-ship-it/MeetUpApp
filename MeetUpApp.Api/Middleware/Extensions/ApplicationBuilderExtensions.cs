using MeetUpApp.Data;
using MeetUpApp.Managers;
using Microsoft.AspNetCore.Hosting;

namespace MeetUpApp.Api.Middleware.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder TryAddFirstUser(this IApplicationBuilder builder)
        {
            using var services = builder
                .ApplicationServices.CreateScope();

            var db = services.ServiceProvider
                .GetRequiredService<AppDataContext>();

            if (!db.User.Any())
            {
                var userManager = services
                    .ServiceProvider.GetRequiredService<UserManager>();

                userManager.AddUser("admin", "Qs3PGVAyyhUXtkRw").Wait();
            }

            return builder;
        }

        public static IApplicationBuilder AddSwaggerWithUI(
            this IApplicationBuilder builder,
            IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                builder.UseSwagger();
                builder.UseSwaggerUI();
            }

            return builder;
        }

        public static IApplicationBuilder AddAuthenticationAndAuthorization(
            this IApplicationBuilder builder)
        {
            builder.UseAuthentication();
            builder.UseAuthorization();

            return builder;
        }

        public static IApplicationBuilder UseSessionWithJwtBearer(
            this IApplicationBuilder builder)
        {
            builder.UseSession();
            builder.UseJwtBearer();

            return builder;
        }

        private static IApplicationBuilder UseJwtBearer(this IApplicationBuilder app)
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
