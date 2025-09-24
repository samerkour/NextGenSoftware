using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace NextGen.Modules.Identity.Identity.Features.VerifyTotp;
internal class VerifyTotpValidator : AbstractValidator<VerifyTotpCommand>
{
    public VerifyTotpValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty().Length(4, 10);
        RuleFor(x => x.DeliveryChannel).IsInEnum();
    }
}
