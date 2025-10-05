using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.Modules.Identity.Users.Features.UpdatingUser;

public record UpdateUserRequest(
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
)
{
}
