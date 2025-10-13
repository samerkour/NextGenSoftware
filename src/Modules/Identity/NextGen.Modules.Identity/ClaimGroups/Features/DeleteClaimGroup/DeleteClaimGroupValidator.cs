using FluentValidation;

namespace NextGen.Modules.Identity.ClaimGroups.Features.DeleteClaimGroup;

public class DeleteClaimGroupValidator : AbstractValidator<DeleteClaimGroupCommand>
{
    public DeleteClaimGroupValidator()
    {
        RuleFor(x => x.GroupId)
            .NotEmpty().WithMessage("GroupId is required.")
            .NotEqual(Guid.Empty).WithMessage("GroupId cannot be an empty GUID.");
    }
}
