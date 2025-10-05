using FluentValidation;

namespace NextGen.Modules.Identity.Identity.Features.Login;

internal class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Captcha)
            .NotEmpty().WithMessage("Captcha is required.");

        RuleFor(x => x.UserNameOrEmail)
            .NotEmpty().WithMessage("UserNameOrEmail cannot be empty.")
            .MaximumLength(256).WithMessage("UserNameOrEmail cannot exceed 256 characters.");

        When(x => !string.IsNullOrWhiteSpace(x.UserNameOrEmail) && x.UserNameOrEmail.Contains('@'), () =>
        {
            RuleFor(x => x.UserNameOrEmail)
                .EmailAddress().WithMessage("Invalid email format.");
        });

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .MaximumLength(128).WithMessage("Password cannot exceed 128 characters.")
            .Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[!@#$%^&*]).+$")
            .WithMessage("Password must contain at least one letter, one number, and one special character.");
    }
}
