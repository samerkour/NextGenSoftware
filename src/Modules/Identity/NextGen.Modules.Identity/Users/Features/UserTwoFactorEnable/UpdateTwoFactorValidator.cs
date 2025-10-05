using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace NextGen.Modules.Identity.Users.Features.UserTwoFactorEnable;
internal class UpdateTwoFactorValidator : AbstractValidator<UpdateTwoFactorCommand>
{
    public UpdateTwoFactorValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");

        RuleFor(x => x.IsTwoFactorEnabled)
            .NotNull()
            .WithMessage("IsEnabled flag is required.");
    }
}
