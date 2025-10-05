using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace NextGen.Modules.Identity;

// Ref: https://docs.duendesoftware.com/identityserver/v5/fundamentals/resources/api_resources/
// https://docs.duendesoftware.com/identityserver/v5/fundamentals/resources/identity/
// https://docs.duendesoftware.com/identityserver/v5/fundamentals/resources/api_scopes/
public static class IdentityServerConfig
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResources.Phone(),
            new IdentityResources.Address(),
            new("roles", "User Roles", new List<string> { "role" })
        };


    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope> { new("NextGen-api", "NextGen.Modules.Inventories API") };

    public static IList<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new ApiResource("nextgenApiResource", "NextGen.Modules.Inventories API Resource")
            {
                Scopes = { "NextGen-api" },
                UserClaims = { JwtClaimTypes.Role, JwtClaimTypes.Name, JwtClaimTypes.Id }
            }
        };


    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new()
            {
                ClientId = "frontend-client",
                ClientName = "Frontend Client",
                RequireClientSecret = false,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "roles",
                    "NextGen-api"
                }
            },
            new()
            {
                ClientId = "oauthClient",
                ClientName = "Example client application using client credentials",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = new List<Secret> { new("SuperSecretPassword".Sha256()) }, // change me!
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "roles",
                    "NextGen-api"
                }
            }
        };
}
