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

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(x => x.ClaimGroupId)
            .NotEmpty().WithMessage("ClaimGroupId cannot be empty.");
    }
}
