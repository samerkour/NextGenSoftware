using FluentValidation;

namespace NextGen.Modules.Identity.Claims.Features.UpdateClaim
{
    public class UpdateClaimValidator : AbstractValidator<UpdateClaimCommand>
    {
        public UpdateClaimValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Claim Id must be provided.")
                .NotEqual(Guid.Empty).WithMessage("Claim Id cannot be an empty GUID.");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Type cannot be empty.")
                .MaximumLength(200).WithMessage("Type cannot exceed 200 characters.");

            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("Value cannot be empty.")
                .MaximumLength(500).WithMessage("Value cannot exceed 500 characters.");

            RuleFor(x => x.ClaimGroupId)
                .NotEmpty().WithMessage("ClaimGroupId cannot be empty.")
                .NotEqual(Guid.Empty).WithMessage("ClaimGroupId cannot be an empty GUID.");
        }
    }
}
