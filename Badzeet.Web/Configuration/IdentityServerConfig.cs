using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Badzeet.Web.Configuration
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };


        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("api", "Badzeet Api")
            };

        public static IEnumerable<Client> Clients(IConfiguration configuration) =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "postman",
                    ClientSecrets = { new Secret(configuration["ApiClients:Postman:secret"].Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { configuration["ApiClients:Postman:redirectUri"] },
                    PostLogoutRedirectUris = { configuration["ApiClients:Postman:postLogoutRedirectUri"] },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api"
                    }
                },

                new Client
                {
                    ClientId = "ng",
                    ClientName = "Angular Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,

                    RedirectUris =           { configuration["ApiClients:Angular:redirectUri"] },
                    PostLogoutRedirectUris = { configuration["ApiClients:Angular:postLogoutRedirectUri"] },
                    AllowedCorsOrigins =     { configuration["ApiClients:Angular:allowedCorsOrigins"] },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api"
                    }
                }
            };
    }
}
