using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace NextGen.Modules.Identity.Users.Features.UserLockoutEnable;

internal class UpdateUserLockoutEnabledValidator : AbstractValidator<UpdateUserLockoutEnabledCommand>
{
    public UpdateUserLockoutEnabledValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");

        RuleFor(x => x.IsLockoutEnabled)
            .NotNull()
            .WithMessage("IsLockoutEnabled flag is required.");
    }
}
