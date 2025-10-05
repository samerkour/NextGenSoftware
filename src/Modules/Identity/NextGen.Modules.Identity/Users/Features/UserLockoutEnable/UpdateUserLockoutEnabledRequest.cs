using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.Modules.Identity.Users.Features.UserLockoutEnable;
public record UpdateUserLockoutEnabledRequest(bool IsLockoutEnabled);
