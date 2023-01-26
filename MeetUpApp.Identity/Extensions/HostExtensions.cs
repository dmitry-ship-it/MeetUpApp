using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MeetUpApp.Identity.Extensions
{
    public static class HostExtensions
    {
        public static IHost UpdateIdentityDbTables(this IHost host, IConfiguration configuration)
        {
            var config = new Config(configuration);

            using var scope = host.Services.CreateScope();
            scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

            using var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            context.Database.Migrate();

            if (!context.Clients.Any())
            {
                foreach (var client in config.Clients)
                {
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in config.IdentityResources)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var apiScope in config.ApiScopes)
                {
                    context.ApiScopes.Add(apiScope.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in config.ApiResources)
                {
                    context.ApiResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            return host;
        }
    }
}
