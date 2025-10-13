using FluentValidation;

namespace NextGen.Modules.Identity.RoleClaims.Features.DeleteRoleClaim
{
    public class DeleteRoleClaimValidator : AbstractValidator<DeleteRoleClaimCommand>
    {
        public DeleteRoleClaimValidator()
        {
            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("RoleId is required.");

            RuleFor(x => x.ClaimId)
                .NotEmpty().WithMessage("ClaimId is required.");
        }
    }
}
