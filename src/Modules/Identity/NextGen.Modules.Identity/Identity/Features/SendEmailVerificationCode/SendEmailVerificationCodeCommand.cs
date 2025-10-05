using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.Identity.Features.SendEmailVerificationCode;

public record SendEmailVerificationCodeCommand(string Email) : ICommand;
