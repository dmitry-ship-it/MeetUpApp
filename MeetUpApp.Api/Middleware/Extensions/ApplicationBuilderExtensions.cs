namespace MeetUpApp.Api.Middleware.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerWithUI(
            this IApplicationBuilder builder)
        {
            var environment = builder.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
            var configuration = builder.ApplicationServices.GetRequiredService<IConfiguration>();

            var authSection = configuration.GetSection("AuthSettings");

            if (environment.IsDevelopment())
            {
                builder.UseSwagger();
                builder.UseSwaggerUI(options =>
                {
                    options.OAuthClientId(authSection["ClientId"]);
                    options.OAuthScopes(authSection["Audience"]);
                });
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
    }
}
