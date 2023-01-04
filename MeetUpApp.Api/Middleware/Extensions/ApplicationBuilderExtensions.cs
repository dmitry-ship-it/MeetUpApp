namespace MeetUpApp.Api.Middleware.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerWithUI(
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

        public static IApplicationBuilder UseAuthenticationAndAuthorization(
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
