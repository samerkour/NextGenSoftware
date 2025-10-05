using NextGen.Modules.Parties.Shared.Clients.Identity.Dtos;

namespace NextGen.Modules.Parties.Shared.Clients.Identity;

// Ref: http://www.kamilgrzybek.com/design/modular-monolith-integration-styles/
// https://docs.microsoft.com/en-us/azure/architecture/patterns/anti-corruption-layer
public interface IIdentityApiClient
{
    Task<GetUserByEmailResponse?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<CreateUserResponse?> CreateUserIdentityAsync(
        CreateUserRequest createUserRequest,
        CancellationToken cancellationToken = default);
}
