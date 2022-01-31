using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace Auth.API
{
    public static class IdentityConfiguration
    {
        public static IEnumerable<Client> GetClients => new List<Client>
        {
            new Client
            {
                ClientId = "Frontend",
                RequireClientSecret = false,
                ClientSecrets = { new Secret("client_secret".ToSha256()) },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                RequirePkce = true,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "ProfileAPI"
                },
                AllowOfflineAccess = true,
                AccessTokenLifetime = 1000
            }
        };

        public static IEnumerable<ApiScope> Get()
        {
            return new[]
            {
                new ApiScope("ProfileAPI")
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            var profileApi = new ApiResource("ProfileAPI")
            {
                Scopes = new List<string> { "ProfileAPI" }
            };

            var resources = new List<ApiResource>
            {
                profileApi
            };

            return resources;
        }

        public static IEnumerable<IdentityResource> GetIdentityResources => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Address()
        };
    }
}