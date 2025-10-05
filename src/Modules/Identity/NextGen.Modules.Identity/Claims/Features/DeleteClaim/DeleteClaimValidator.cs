using FluentValidation;

namespace NextGen.Modules.Identity.Claims.Features.DeleteClaim;

public class DeleteClaimValidator : AbstractValidator<DeleteClaimCommand>
{
    public DeleteClaimValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Claim Id must be provided.")
            .NotEqual(Guid.Empty).WithMessage("Claim Id cannot be an empty GUID.");
    }
}
