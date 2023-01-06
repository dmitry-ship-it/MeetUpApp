using IdentityServer4.Services;
using Microsoft.EntityFrameworkCore;

namespace MeetUpApp.Identity.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPreconfiguredIdentityServer(
            this IServiceCollection services, IConfiguration configuration)
        {
            void ConfigureDbContext(DbContextOptionsBuilder x) =>
                x.UseSqlServer(configuration.GetConnectionString("DefaultDb"),
                o => o.MigrationsAssembly(typeof(Program).Assembly.FullName));

            services.AddIdentityServer()
                .AddConfigurationStore(options => options.ConfigureDbContext = ConfigureDbContext)
                .AddOperationalStore(options => options.ConfigureDbContext = ConfigureDbContext)
                .AddDeveloperSigningCredential();

            return services;
        }

        public static IServiceCollection AddCustomCorsPolicy(
            this IServiceCollection services)
        {
            return services.AddSingleton<ICorsPolicyService>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<DefaultCorsPolicyService>>();

                return new DefaultCorsPolicyService(logger)
                {
                    AllowAll = true
                };
            });
        }
    }
}
