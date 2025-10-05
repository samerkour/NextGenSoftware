using FluentValidation;

namespace NextGen.Modules.Identity.Users.Features.UpdatingUserLockout;

internal class UpdateUserLockoutValidator : AbstractValidator<UpdateUserLockoutCommand>
{
    public UpdateUserLockoutValidator()
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(v => v.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");

        RuleFor(x => x.LockoutEnd)
           .Must(lockoutEnd => !lockoutEnd.HasValue || lockoutEnd.Value >= DateTimeOffset.UtcNow)
           .WithMessage("LockoutEnd must be in the present or future.");
    }
}
