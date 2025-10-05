using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace NextGen.Modules.Identity.Identity.Features.SendTOTP;
public class SendTotpValidator : AbstractValidator<SendTotpCommand>
{
    public SendTotpValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");

        RuleFor(x => x.DeliveryChannel)
            .IsInEnum()
            .WithMessage("Delivery channel must be a valid option (e.g., Email or Phone).");
    }
}
