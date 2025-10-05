using FluentValidation;

namespace NextGen.Modules.Identity.Users.Features.RegisteringUser;

internal class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserValidator()
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(v => v.FirstName)
            .NotEmpty()
            .WithMessage("FirstName is required.");

        RuleFor(v => v.LastName)
            .NotEmpty()
            .WithMessage("LastName is required.");

        RuleFor(v => v.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress();

        RuleFor(v => v.UserName)
            .NotEmpty()
            .WithMessage("UserName is required.");

        RuleFor(v => v.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(6)
            .WithMessage("New password must be at least 6 characters long.");

        RuleFor(x => x.ConfirmPassword)
          .Equal(x => x.Password)
          .WithMessage("The new password and confirmation password do not match.");

        RuleFor(v => v.Roles).Custom((roles, c) =>
        {
            if (roles != null &&
                !roles.All(x => x.Contains(Constants.Role.Admin, StringComparison.Ordinal) ||
                                x.Contains(Constants.Role.User, StringComparison.Ordinal)))
            {
                c.AddFailure("Invalid roles.");
            }
        });
    }
}
