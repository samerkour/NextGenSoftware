using FluentValidation;

namespace NextGen.Modules.Identity.ClaimGroups.Features.GetClaimGroups;

internal class GetClaimGroupsValidator : AbstractValidator<GetClaimGroupsQuery>
{
    public GetClaimGroupsValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page should be greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageSize should be greater than or equal to 1.");
    }
}
