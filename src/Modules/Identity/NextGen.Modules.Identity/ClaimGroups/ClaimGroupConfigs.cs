using Asp.Versioning.Builder;
using Asp.Versioning.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NextGen.Modules.Identity.ClaimGroups.Features.GetClaimGroupById;
using NextGen.Modules.Identity.ClaimGroups.Features.GetClaimGroups;
using NextGen.Modules.Identity.Claims;

namespace NextGen.Modules.Identity.ClaimGroups
{
    internal static class ClaimGroupConfigs
    {
        public const string Tag = "claim-groups";
        public const string ClaimGroupsPrefixUri = $"{ClaimsModuleConfiguration.ClaimsGroupModulePrefixUri}/{Tag}";

        public static ApiVersionSet VersionSet { get; private set; } = default!;

        internal static IServiceCollection AddClaimGroupServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services;
        }

        internal static IEndpointRouteBuilder MapClaimGroupEndpoints(this IEndpointRouteBuilder endpoints)
        {
            VersionSet = endpoints.NewApiVersionSet(Tag).Build();

            endpoints.MapGetClaimGroupsEndpoint();
            endpoints.MapGetClaimGroupByIdEndpoint();
            endpoints.MapCreateClaimGroupEndpoint();
            endpoints.MapUpdateClaimGroupEndpoint();
            endpoints.MapGetClaimsByGroupEndpoint();
            endpoints.MapRemoveClaimFromGroupEndpoint();
            endpoints.MapAssignClaimGroupToRoleEndpoint();
            endpoints.MapUnassignClaimGroupFromRoleEndpoint();
            endpoints.MapDeleteClaimGroupEndpoint();
            endpoints.MapAddClaimToGroupEndpoint();

            return endpoints;
        }

    }
}
