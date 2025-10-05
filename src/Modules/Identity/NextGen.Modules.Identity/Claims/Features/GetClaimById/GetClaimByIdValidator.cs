using FluentValidation;

namespace NextGen.Modules.Identity.Claims.Features.GetClaimById
{
    public class GetClaimByIdValidator : AbstractValidator<GetClaimByIdQuery>
    {
        public GetClaimByIdValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Claim Id must be provided.")
                .NotEqual(Guid.Empty).WithMessage("Claim Id cannot be an empty GUID.");
        }
    }
}
