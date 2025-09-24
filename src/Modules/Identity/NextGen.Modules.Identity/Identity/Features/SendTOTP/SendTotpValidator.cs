using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace NextGen.Modules.Identity.Identity.Features.SendTOTP;
internal class SendTotpValidator : AbstractValidator<SendTotpCommand>
{
    public SendTotpValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.DeliveryChannel).IsInEnum();
    }
}
