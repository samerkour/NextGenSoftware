using FluentValidation;

namespace NextGen.Modules.Identity.Roles.Features.AssignClaimGroupToRole
{
    public class AssignClaimGroupToRoleValidator : AbstractValidator<AssignClaimGroupToRoleCommand>
    {
        public AssignClaimGroupToRoleValidator()
        {
            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("RoleId is required.");

            RuleFor(x => x.ClaimGroupId)
                .NotEmpty().WithMessage("ClaimGroupId is required.");
        }
    }
}
