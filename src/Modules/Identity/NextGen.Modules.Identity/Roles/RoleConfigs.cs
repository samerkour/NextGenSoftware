using Asp.Versioning.Builder;
using Asp.Versioning.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NextGen.Modules.Identity.Roles.Features.CreateRole;
using NextGen.Modules.Identity.Roles.Features.DeleteRole;
using NextGen.Modules.Identity.Roles.Features.GetRoleById;
using NextGen.Modules.Identity.Roles.Features.GetRoleClaimGroups;
using NextGen.Modules.Identity.Roles.Features.GetRoles;
using NextGen.Modules.Identity.Roles.Features.UpdateRole;

namespace NextGen.Modules.Identity.Roles
{
    internal static class RoleConfigs
    {
        public const string Tag = "roles";
        public const string RolesPrefixUri = $"{RolesModuleConfiguration.RolesModulePrefixUri}/{Tag}";
        public static ApiVersionSet VersionSet { get; private set; } = default!;

        internal static IServiceCollection AddRoleServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services;
        }

        internal static IEndpointRouteBuilder MapRoleEndpoints(this IEndpointRouteBuilder endpoints)
        {
            VersionSet = endpoints.NewApiVersionSet(Tag).Build();

            endpoints.MapCreateRoleEndpoint();
            endpoints.MapUpdateRoleEndpoint();
            endpoints.MapGetRoleByIdEndpoint();
            endpoints.MapDeleteRoleEndpoint();
            endpoints.MapGetRolesEndpoint();
            endpoints.MapGetRoleClaimGroupsEndpoint();
            return endpoints;
        }
    }
}
