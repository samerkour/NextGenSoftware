using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.Identity.Features.ResetPassword;

public record ResetPasswordCommand(
    Guid UserId,
    string ResetToken,
    string NewPassword,
    string ConfirmNewPassword
) : ITxUpdateCommand;
