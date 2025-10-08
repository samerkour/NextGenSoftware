using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.Identity.Features.Logout;

public record RevokeRefreshTokenCommand(string RefreshToken) : ICommand;
