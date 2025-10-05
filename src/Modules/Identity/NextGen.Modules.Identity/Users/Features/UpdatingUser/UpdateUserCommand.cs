using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.Users.Features.UpdatingUser;

public record UpdateUserCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string? MiddleName = null,
    DateTime? DateOfBirth = null,
    string? PlaceOfBirth = null,
    string? ProfileImagePath = null,
    string? Country = null,
    string? City = null,
    string? State = null,
    string? Address = null,
    string? PostalCode = null
) : ITxUpdateCommand<UpdateUserResponse>;
