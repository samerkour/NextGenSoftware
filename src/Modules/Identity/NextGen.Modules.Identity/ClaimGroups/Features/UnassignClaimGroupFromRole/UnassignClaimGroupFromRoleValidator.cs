using FluentValidation;

namespace NextGen.Modules.Identity.ClaimGroups.Features.UnassignClaimGroupFromRole
{
    public class UnassignClaimGroupFromRoleValidator : AbstractValidator<UnassignClaimGroupFromRoleCommand>
    {
        public UnassignClaimGroupFromRoleValidator()
        {
            RuleFor(x => x.ClaimGroupId)
                .NotEmpty().WithMessage("ClaimGroupId is required.");

            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("RoleId is required.");
        }
    }
}
