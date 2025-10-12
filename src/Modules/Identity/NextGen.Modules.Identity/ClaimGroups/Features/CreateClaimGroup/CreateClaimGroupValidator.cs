using FluentValidation;

namespace NextGen.Modules.Identity.ClaimGroups.Features.CreateClaimGroup
{
    public class CreateClaimGroupValidator : AbstractValidator<CreateClaimGroupCommand>
    {
        public CreateClaimGroupValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(500);
        }
    }
}
