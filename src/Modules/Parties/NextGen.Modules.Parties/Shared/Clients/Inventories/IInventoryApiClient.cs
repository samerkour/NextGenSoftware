using NextGen.Modules.Parties.Shared.Clients.Inventorys.Dtos;

namespace NextGen.Modules.Parties.Shared.Clients.Inventorys;

// Ref: http://www.kamilgrzybek.com/design/modular-monolith-integration-styles/
// https://docs.microsoft.com/en-us/azure/architecture/patterns/anti-corruption-layer
public interface IInventoryApiClient
{
    Task<GetProductByIdResponse?> GetProductByIdAsync(long id, CancellationToken cancellationToken = default);
}
