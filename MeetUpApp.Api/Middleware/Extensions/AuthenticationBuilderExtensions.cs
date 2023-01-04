using Microsoft.AspNetCore.Authentication;
using System.Text;

namespace MeetUpApp.Api.Middleware.Extensions
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddPreconfiguredJwtBearer(
            this AuthenticationBuilder builder,
            IConfiguration configuration)
        {
            var authSection = configuration.GetRequiredSection("AuthSettings");

            var key = Encoding.Default.GetBytes(authSection["Token"]!);
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
