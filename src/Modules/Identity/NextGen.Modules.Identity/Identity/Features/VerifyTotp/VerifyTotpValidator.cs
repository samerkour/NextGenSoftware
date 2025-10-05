using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace NextGen.Modules.Identity.Identity.Features.VerifyTotp;
public class VerifyTotpValidator : AbstractValidator<VerifyTotpCommand>
{
    public VerifyTotpValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Verification code is required.")
            .Length(4, 10)
            .WithMessage("Verification code must be between 4 and 10 characters long.");

        RuleFor(x => x.DeliveryChannel)
            .IsInEnum()
            .WithMessage("Delivery channel must be a valid option (e.g., Email or Phone).");
    }
}
