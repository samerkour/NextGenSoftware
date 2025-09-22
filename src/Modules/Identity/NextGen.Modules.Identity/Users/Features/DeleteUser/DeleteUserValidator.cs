using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace NextGen.Modules.Identity.Users.Features.DeleteUser;

internal class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserValidator()
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(v => v.UserId)
            .NotEmpty();
    }
}
