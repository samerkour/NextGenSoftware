// RoleClaimConfigs.cs
using Asp.Versioning.Builder;
using Asp.Versioning.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NextGen.Modules.Identity.RoleClaims.Features.AddClaimToRole;
using NextGen.Modules.Identity.RoleClaims.Features.DeleteRoleClaim;
using NextGen.Modules.Identity.RoleClaims.Features.GetRoleClaims;


namespace NextGen.Modules.Identity.RoleClaims
{
    internal static class RoleClaimConfigs
    {
        public const string Tag = "role-claims";
        public const string RoleClaimsPrefixUri = $"{RoleClaimsModuleConfiguration.RoleClaimsModulePrefixUri}/{Tag}";

        public static ApiVersionSet VersionSet { get; private set; } = default!;

        internal static IServiceCollection AddRoleClaimsServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services;
        }

        internal static IEndpointRouteBuilder MapRoleClaimEndpoints(this IEndpointRouteBuilder endpoints)
        {
            VersionSet = endpoints.NewApiVersionSet(Tag).Build();

            // Endpoints
            endpoints.MapGetRoleClaimsEndpoint();       // GET /roles/{roleId}/claims
            endpoints.MapAddClaimToRoleEndpoint();      // POST /roles/{roleId}/claims
            endpoints.MapDeleteRoleClaimEndpoint();     // DELETE /roles/{roleId}/claims/{claimId}

            return endpoints;
        }
    }
}
