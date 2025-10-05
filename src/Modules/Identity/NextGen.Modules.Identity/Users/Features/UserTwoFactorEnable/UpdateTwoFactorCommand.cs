using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.Users.Features.UserTwoFactorEnable;
public record UpdateTwoFactorCommand(Guid UserId, bool IsTwoFactorEnabled)
    : ITxUpdateCommand<bool>;
