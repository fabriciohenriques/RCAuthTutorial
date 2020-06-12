using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                //new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "rc.scope",
                    UserClaims =
                    {
                        "rc.grandma"
                    },
                },
            };

        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource>
            {
                new ApiResource("ApiOne"),
                new ApiResource("ApiTwo", new string[] { "rc.api.grandma" }),
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>()
            {
                new Client
                {
                    AllowedCorsOrigins = { "https://localhost:44335" },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {"ApiOne"},
                    ClientId = "client_id",
                    ClientSecrets = {new Secret("client_secret".ToSha256())},
                },
                new Client
                {
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowOfflineAccess = true,
                    AllowedScopes =
                    {
                        "ApiOne",
                        "ApiTwo",
                        IdentityServerConstants.StandardScopes.OpenId,
                        //IdentityServerConstants.StandardScopes.Profile,
                        "rc.scope",
                    },
                    //AlwaysIncludeUserClaimsInIdToken = true,
                    ClientId = "client_id_mvc",
                    ClientSecrets = {new Secret("client_secret_mvc".ToSha256())},
                    RedirectUris = { "https://localhost:44385/signin-oidc" },
                    RequireConsent = false,
                },
                new Client
                {
                    AccessTokenLifetime = 1,
                    AllowAccessTokensViaBrowser = true,
                    AllowedCorsOrigins = { "https://localhost:44335" },
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes =
                    {
                        "ApiOne",
                        "ApiTwo",
                        IdentityServerConstants.StandardScopes.OpenId,
                        "rc.scope",
                    },
                    ClientId = "client_id_js",
                    RedirectUris = { "https://localhost:44335/home/signin" },
                    RequireConsent = false,
                },
            };
    }
}
