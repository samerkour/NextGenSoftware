using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.Identity.Features.VerifyEmail;

public record VerifyEmailCommand(string Email, string Code) : ICommand;
