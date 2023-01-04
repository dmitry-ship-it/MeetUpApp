using IdentityServer4;
using IdentityServer4.Models;

namespace MeetUpApp.Identity
{
    public class Config
    {
        private readonly IConfigurationSection configuration;

        public Config(IConfiguration configuration)
        {
            this.configuration = configuration.GetSection("Identity");
        }

        public IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
            };

        public IEnumerable<ApiScope> ApiScopes =>
            ApiScopesNames.Select(s => new ApiScope(s));

        public IEnumerable<ApiResource> ApiResources =>
            new[]
            {
                new ApiResource()
                {
                    Name = configuration["ApiResourceName"],
                    Scopes = ApiScopesNames,
                }
            };

        public IEnumerable<Client> Clients =>
            new[]
            {
                new Client
                {
                    ClientId = configuration["ClientId"],
                    ClientName = configuration["ClientName"],
                    ClientSecrets = new[]
                    {
                        new Secret(configuration["ClientSecret"].Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new[]
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                    }.Concat(ApiScopesNames).ToArray(),
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = TimeSpan.FromHours(2).Seconds
                }
            };

        private string[] ApiScopesNames => configuration.GetSection("ApiScopes").Get<string[]>();
    }
}
