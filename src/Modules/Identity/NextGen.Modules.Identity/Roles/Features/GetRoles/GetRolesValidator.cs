using FluentValidation;

namespace NextGen.Modules.Identity.Roles.Features.GetRoles;

internal class GetRolesValidator : AbstractValidator<GetRolesQuery>
{
    public GetRolesValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page should be greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageSize should be greater than or equal to 1.");
    }
}
