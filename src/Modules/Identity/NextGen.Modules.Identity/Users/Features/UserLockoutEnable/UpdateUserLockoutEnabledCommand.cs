using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.Users.Features.UserLockoutEnable;
public record UpdateUserLockoutEnabledCommand(Guid UserId, bool IsLockoutEnabled)
    : ITxUpdateCommand<bool>;
