using Microsoft.AspNetCore.Authentication;

namespace MeetUpApp.Api.Middleware.Extensions
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddPreconfiguredJwtBearer(
            this AuthenticationBuilder builder,
            IConfiguration configuration)
        {
            var authSection = configuration.GetRequiredSection("AuthSettings");

            var authority = authSection["Authority"];
            var audience = authSection["Audience"];

            return builder.AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.Authority = authority;
                options.Audience = audience;
                options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                options.TokenValidationParameters.ValidateAudience = false;
            });
        }
    }
}
