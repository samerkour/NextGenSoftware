using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Abstractions.CQRS.Event.Internal;

namespace NextGen.Modules.Identity.Users.Features.UserTwoFactorEnable.Events.Integration;
public record UserTwoFactorUpdated(Guid UserId, bool Enabled, DateTime? EnabledOn);
