using Serilog;

namespace MeetUpApp.Api.Middleware.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UsePreconfiguredSerilog(
            this IHostBuilder builder, IConfiguration configuration)
        {
            Serilog.ILogger logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            return builder.UseSerilog(logger);
        }
    }
}
