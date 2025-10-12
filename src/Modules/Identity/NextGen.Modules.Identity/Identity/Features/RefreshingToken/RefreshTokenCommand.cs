using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.Identity.Features.RefreshingToken;

public record RefreshTokenCommand(string AccessTokenData, string RefreshTokenData) : ICommand<RefreshTokenResponse>;
