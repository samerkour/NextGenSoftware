using FluentValidation;

namespace NextGen.Modules.Identity.Claims.Features.CreateClaim;

public class CreateClaimValidator : AbstractValidator<CreateClaimCommand>
{
    public CreateClaimValidator()
    {
        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type cannot be empty.")
            .MaximumLength(200).WithMessage("Type cannot exceed 200 characters.");

        RuleFor(x => x.Value)
            .NotEmpty().WithMessage("Value cannot be empty.")
            .MaximumLength(500).WithMessage("Value cannot exceed 500 characters.");

        RuleFor(x => x.ClaimGroupId)
            .NotEmpty().WithMessage("ClaimGroupId cannot be empty.");
    }
}
