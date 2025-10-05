using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.Modules.Identity.Identity.Features.ResetPassword.Events.Integration;
public record UserPasswordReset(Guid UserId, DateTime ResetOn);
