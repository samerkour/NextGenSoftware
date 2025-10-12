using FluentValidation;

namespace NextGen.Modules.Identity.ClaimGroups.Features.GetClaimGroupById
{
    public class GetClaimGroupByIdValidator : AbstractValidator<GetClaimGroupByIdQuery>
    {
        public GetClaimGroupByIdValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required.");
        }
    }
}
