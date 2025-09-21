using BuildingBlocks.Resiliency.Extensions;
using NextGen.Modules.Parties.Shared.Clients.Inventorys;
using NextGen.Modules.Parties.Shared.Clients.Identity;

namespace NextGen.Modules.Parties.Shared.Extensions.ServiceCollectionExtensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomHttpClients(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<IdentityApiClientOptions>().Bind(configuration.GetSection(
                $"{PartiesModuleConfiguration.ModuleName}:{nameof(IdentityApiClientOptions)}"))
            .ValidateDataAnnotations();

        services.AddOptions<InventorysApiClientOptions>().Bind(
                configuration.GetSection(
                    $"{PartiesModuleConfiguration.ModuleName}:{nameof(InventorysApiClientOptions)}"))
            .ValidateDataAnnotations();

        services.AddHttpApiClient<IInventoryApiClient, InventoryApiClient>();
        services.AddHttpApiClient<IIdentityApiClient, IdentityApiClient>();

        return services;
    }
}
