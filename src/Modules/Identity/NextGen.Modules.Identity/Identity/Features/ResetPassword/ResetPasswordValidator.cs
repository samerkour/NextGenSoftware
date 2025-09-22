using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace NextGen.Modules.Identity.Identity.Features.ResetPassword;
internal class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordValidator()
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(x => x.UserId).NotEmpty();

        RuleFor(x => x.ResetToken)
            .NotEmpty()
            .WithMessage("Reset token is required.");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(6)
            .WithMessage("New password must be at least 6 characters long.");

        RuleFor(x => x.ConfirmNewPassword)
            .Equal(x => x.NewPassword)
            .WithMessage("The new password and confirmation password do not match.");
    }
}
