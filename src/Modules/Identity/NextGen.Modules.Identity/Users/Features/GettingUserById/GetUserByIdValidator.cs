using FluentValidation;

namespace NextGen.Modules.Identity.Users.Features.GettingUserById;

internal class GetUserByIdValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
