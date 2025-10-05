using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.Modules.Identity.Users.Features.UserLockoutEnable.Events.Integration;

public record UserLockoutEnabledUpdated(Guid UserId, bool LockoutEnabled, DateTime? LockoutEnabledOn);
