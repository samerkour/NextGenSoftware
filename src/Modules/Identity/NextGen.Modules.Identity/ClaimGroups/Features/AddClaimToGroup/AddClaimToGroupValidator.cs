using FluentValidation;

namespace NextGen.Modules.Identity.ClaimGroups.Features.AddClaimToGroup
{
    public class AddClaimToGroupValidator : AbstractValidator<AddClaimToGroupCommand>
    {
        public AddClaimToGroupValidator()
        {
            RuleFor(x => x.GroupId)
                .NotEmpty().WithMessage("GroupId is required.");

            RuleFor(x => x.ClaimId)
                .NotEmpty().WithMessage("ClaimId is required.");
        }
    }
}
