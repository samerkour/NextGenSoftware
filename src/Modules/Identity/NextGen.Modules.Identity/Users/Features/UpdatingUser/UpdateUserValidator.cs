using FluentValidation;

namespace NextGen.Modules.Identity.Users.Features.UpdatingUser;

internal class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(v => v.Id)
            .NotEmpty()
            .WithMessage("User Id is required.");

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
    }
}
