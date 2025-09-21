using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextGen.Modules.Identity.Users.Dtos;

namespace NextGen.Modules.Identity.Users.Features.UpdatingUser;

internal record UpdateUserResponse(IdentityUserDto? UserIdentity);
