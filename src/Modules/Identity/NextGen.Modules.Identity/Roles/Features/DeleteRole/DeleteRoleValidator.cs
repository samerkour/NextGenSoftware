using FluentValidation;

namespace NextGen.Modules.Identity.Roles.Features.DeleteRole
{
    public class DeleteRoleValidator : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Role Id must be provided.")
                .NotEqual(Guid.Empty)
                .WithMessage("Role Id cannot be an empty GUID.");
        }
    }
}
