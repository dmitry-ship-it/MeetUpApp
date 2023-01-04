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
    }
}
