using FluentValidation;

namespace NextGen.Modules.Identity.ClaimGroups.Features.GetClaimGroups
{
    public class GetClaimGroupsValidator : AbstractValidator<GetClaimGroupsQuery>
    {
        public GetClaimGroupsValidator()
        {
            RuleFor(x => x.SearchTerm)
                .MaximumLength(100)
                .WithMessage("Search term cannot exceed 100 characters.");
        }
    }
}
