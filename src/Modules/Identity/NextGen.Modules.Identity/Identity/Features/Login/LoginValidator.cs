using FluentValidation;
using Microsoft.Extensions.Localization;
using NextGen.Modules.Identity.Identity.Features.Login;

namespace NextGen.Modules.Identity.Identity.Features.Login;

internal class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator(IStringLocalizer<LoginValidator> localizer)
    {
        RuleFor(x => x.Captcha)
            .NotEmpty().WithMessage(localizer["CaptchaRequired"]);

        RuleFor(x => x.UserNameOrEmail)
            .NotEmpty().WithMessage(localizer["UserNameOrEmailRequired"])
            .MaximumLength(256).WithMessage(localizer["UserNameOrEmailMaxLength"]);

        When(x => !string.IsNullOrWhiteSpace(x.UserNameOrEmail) && x.UserNameOrEmail.Contains('@'), () =>
        {
            RuleFor(x => x.UserNameOrEmail)
                .EmailAddress().WithMessage(localizer["InvalidEmailFormat"]);
        });

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(localizer["PasswordRequired"])
            .MinimumLength(8).WithMessage(localizer["PasswordMinLength"])
            .MaximumLength(128).WithMessage(localizer["PasswordMaxLength"])
            .Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[!@#$%^&*]).+$")
            .WithMessage(localizer["PasswordComplexity"]);
    }
}
