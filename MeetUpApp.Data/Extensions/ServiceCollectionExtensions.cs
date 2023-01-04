using MeetUpApp.Data.DAL;
using MeetUpApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeetUpApp.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDbContextWithRepositories(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDataContext>(options =>
                options.UseSqlServer(configuration
                    .GetConnectionString("DefaultDb")));

            services.AddScoped<IRepository<Meetup>, MeetupRepository>();

            return services;
        }
    }
}
