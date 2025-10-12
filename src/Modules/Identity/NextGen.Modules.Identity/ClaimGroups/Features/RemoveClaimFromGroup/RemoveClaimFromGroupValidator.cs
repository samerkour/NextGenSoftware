using FluentValidation;

namespace NextGen.Modules.Identity.ClaimGroups.Features.RemoveClaimFromGroup
{
    public class RemoveClaimFromGroupValidator : AbstractValidator<RemoveClaimFromGroupRequest>
    {
        public RemoveClaimFromGroupValidator()
        {
            RuleFor(x => x.GroupId)
                .NotEmpty().WithMessage("GroupId is required.");

            RuleFor(x => x.ClaimId)
                .NotEmpty().WithMessage("ClaimId is required.");
        }
    }
}
