using Asp.Versioning.Builder;
using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Abstractions.Web.Module;
using NextGen.Modules.Parties.Parties.Data;

namespace NextGen.Modules.Parties.Parties;

internal static class PartiesConfigs
{
    public const string Tag = "Parties";
    public const string PartiesPrefixUri = $"{PartiesModuleConfiguration.PartyModulePrefixUri}";
    public static ApiVersionSet VersionSet { get; private set; } = default!;

    internal static IServiceCollection AddPartiesServices(this IServiceCollection services)
    {
        services.AddScoped<IDataSeeder, PartiesDataSeeder>();

        return services;
    }

    internal static IEndpointRouteBuilder MapPartiesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        VersionSet = endpoints.NewApiVersionSet(Tag).Build();

        // Routes mapped by conventions, with implementing `IMinimalEndpointDefinition` but we can map them here without convention.
        return endpoints;
    }
}
