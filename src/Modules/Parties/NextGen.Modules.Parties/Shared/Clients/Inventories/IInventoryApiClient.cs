using NextGen.Modules.Parties.Shared.Clients.Inventories.Dtos;

namespace NextGen.Modules.Parties.Shared.Clients.Inventories;

// Ref: http://www.kamilgrzybek.com/design/modular-monolith-integration-styles/
// https://docs.microsoft.com/en-us/azure/architecture/patterns/anti-corruption-layer
public interface IInventoryApiClient
{
    Task<GetProductByIdResponse?> GetProductByIdAsync(long id, CancellationToken cancellationToken = default);
}
