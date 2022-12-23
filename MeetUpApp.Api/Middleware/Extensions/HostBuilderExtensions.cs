using Serilog;

namespace MeetUpApp.Api.Middleware.Extensions
{
    public static class HostBuilderExtensions
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
    }
}
