using FluentValidation;

namespace NextGen.Modules.Identity.Identity.Features.Login;

internal class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        // Validations to prevent malformed inputs, aligns with security best practices, and reduces the risk of injection attacks(though ASP.NET Core Identity sanitizes inputs).
        RuleFor(x => x.UserNameOrEmail)
            .NotEmpty().WithMessage("UserNameOrEmail cannot be empty")
            .MaximumLength(256).WithMessage("UserNameOrEmail cannot exceed 256 characters")
            .When(x => x.UserNameOrEmail.Contains('@'))
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .MaximumLength(128).WithMessage("Password cannot exceed 128 characters")
            .Matches(@"[A-Za-z0-9!@#$%^&*]").WithMessage("Password must contain at least one letter, number, or special character");
    }
}
