using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Core.Exception.Types;

namespace NextGen.Modules.Identity.Users.Features.UserLockoutEnable;
public class UpdateUserLockoutEnabledException : BadRequestException
{
    public UpdateUserLockoutEnabledException(string error) : base(error) { }
}
