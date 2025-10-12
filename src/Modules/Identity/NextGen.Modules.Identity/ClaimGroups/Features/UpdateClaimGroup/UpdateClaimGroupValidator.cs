using FluentValidation;

namespace NextGen.Modules.Identity.ClaimGroups.Features.UpdateClaimGroup
{
    public class UpdateClaimGroupValidator : AbstractValidator<UpdateClaimGroupCommand>
    {
        public UpdateClaimGroupValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(500);
        }
    }
}
