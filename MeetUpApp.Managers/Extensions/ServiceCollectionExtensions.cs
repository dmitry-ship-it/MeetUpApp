using Microsoft.Extensions.DependencyInjection;

namespace MeetUpApp.Managers.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddManagers(
            this IServiceCollection services)
        {
            services.AddScoped<MeetupManager>();

            return services;
        }
    }
}
