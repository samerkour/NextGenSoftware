using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using FluentValidation;

namespace NextGen.Modules.Identity.Users.Features.DeleteUser;

internal class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserValidator()
    {
        RuleFor(x => x.UserId)
              .NotEmpty()
              .WithMessage("UserId is required.");

        RuleFor(x => x.IsDeleted)
          .NotNull()
          .WithMessage("IsDeleted flag is required.");
    }
}
