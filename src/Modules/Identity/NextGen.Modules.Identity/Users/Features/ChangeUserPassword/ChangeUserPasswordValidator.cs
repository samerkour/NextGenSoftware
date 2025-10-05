using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;

namespace NextGen.Modules.Identity.Users.Features.ChangeUserPassword;

internal class ChangeUserPasswordValidator : AbstractValidator<ChangeUserPasswordCommand>
{
    public ChangeUserPasswordValidator()
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(v => v.UserId)
           .NotEmpty()
           .WithMessage("UserId is required.");

        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .WithMessage("Current password is required.")
            .MinimumLength(6)
            .WithMessage("New password must be at least 6 characters long.");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(6)
            .WithMessage("New password must be at least 6 characters long.");

        RuleFor(x => x.ConfirmNewPassword)
            .Equal(x => x.NewPassword)
            .WithMessage("The new password and confirmation password do not match.");
    }
}

