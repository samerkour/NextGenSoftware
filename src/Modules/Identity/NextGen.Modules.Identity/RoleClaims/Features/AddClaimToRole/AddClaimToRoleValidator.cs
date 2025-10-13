using FluentValidation;

namespace NextGen.Modules.Identity.Roles.Features.AddClaimToRole
{
    public class AddClaimToRoleValidator : AbstractValidator<AddClaimToRoleCommand>
    {
        public AddClaimToRoleValidator()
        {
            RuleFor(x => x.RoleId)
                .NotEmpty()
                .WithMessage("RoleId cannot be empty.");

            RuleFor(x => x.ClaimId)
                .NotEmpty()
                .WithMessage("ClaimId cannot be empty.");
        }
    }
}
