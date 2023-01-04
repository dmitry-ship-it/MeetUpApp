using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MeetUpApp.Identity.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPreconfiguredIdentityServer(
            this IServiceCollection services, IConfiguration configuration)
        {
            var assemblyName = typeof(Program).Assembly.GetName().Name;

            void ConfigureDbContext(DbContextOptionsBuilder x) =>
                x.UseSqlServer(configuration.GetConnectionString("DefaultDb"),
                o => o.MigrationsAssembly(assemblyName));

            services.AddIdentityServer()
                .AddConfigurationStore(options => options.ConfigureDbContext = ConfigureDbContext)
                .AddOperationalStore(options => options.ConfigureDbContext = ConfigureDbContext)
                .AddDeveloperSigningCredential();

            return services;
        }
    }
}
