using FluentValidation;

namespace NextGen.Modules.Identity.Roles.Features.UpdateRole;

public class UpdateRoleValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Role Id must be provided.")
            .NotEqual(Guid.Empty).WithMessage("Role Id cannot be an empty GUID.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Role name cannot be empty.")
            .MaximumLength(200).WithMessage("Role name cannot exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");
    }
}
