using System.Net.Http.Json;
using Ardalis.GuardClauses;
using NextGen.Modules.Parties.Shared.Clients.Inventories.Dtos;
using Microsoft.Extensions.Options;

namespace NextGen.Modules.Parties.Shared.Clients.Inventories;

// Ref: http://www.kamilgrzybek.com/design/modular-monolith-integration-styles/
// https://docs.microsoft.com/en-us/azure/architecture/patterns/anti-corruption-layer
public class InventoryApiClient : IInventoryApiClient
{
    private readonly HttpClient _httpClient;
    private readonly InventoriesApiClientOptions _options;

    public InventoryApiClient(HttpClient httpClient, IOptions<InventoriesApiClientOptions> options)
    {
        _httpClient = Guard.Against.Null(httpClient, nameof(httpClient));
        _options = Guard.Against.Null(options.Value, nameof(options));

        if (string.IsNullOrEmpty(_options.BaseApiAddress) == false)
            _httpClient.BaseAddress = new Uri(_options.BaseApiAddress);
        _httpClient.Timeout = new TimeSpan(0, 0, 30);
        _httpClient.DefaultRequestHeaders.Clear();
    }


    public async Task<GetProductByIdResponse?> GetProductByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.NegativeOrZero(id, nameof(id));

        var response = await _httpClient.GetFromJsonAsync<GetProductByIdResponse>(
            $"{_options.ProductsEndpoint}/{id}",
            cancellationToken);

        return response;
    }
}
