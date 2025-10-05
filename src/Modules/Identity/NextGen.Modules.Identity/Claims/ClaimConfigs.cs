using Asp.Versioning.Builder;
using Asp.Versioning.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NextGen.Modules.Identity.Claims.Features.CreateClaim;
using NextGen.Modules.Identity.Claims.Features.DeleteClaim;
using NextGen.Modules.Identity.Claims.Features.GetClaimById;
using NextGen.Modules.Identity.Claims.Features.GetClaims;
using NextGen.Modules.Identity.Claims.Features.UpdateClaim;

namespace NextGen.Modules.Identity.Claims
{
    internal static class ClaimConfigs
    {
        public const string Tag = "claims";
        public const string ClaimsPrefixUri = $"{ClaimsModuleConfiguration.ClaimsModulePrefixUri}/{Tag}";

        public static ApiVersionSet VersionSet { get; private set; } = default!;


        internal static IServiceCollection AddClaimsServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services;
        }

        internal static IEndpointRouteBuilder MapClaimEndpoints(this IEndpointRouteBuilder endpoints)
        {
            VersionSet = endpoints.NewApiVersionSet(Tag).Build();

            endpoints.MapCreateClaimEndpoint();
            endpoints.MapDeleteClaimEndpoint();
            endpoints.MapUpdateClaimEndpoint();
            endpoints.MapGetClaimsEndpoint();
            endpoints.MapGetClaimByIdEndpoint();

            return endpoints;
        }
    }
}
