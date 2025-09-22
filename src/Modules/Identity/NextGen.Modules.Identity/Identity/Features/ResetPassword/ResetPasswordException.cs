using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Core.Exception.Types;

namespace NextGen.Modules.Identity.Identity.Features.ResetPassword;
public class ResetPasswordException : BadRequestException
{
    public ResetPasswordException(string error)
        : base($"Failed to reset password: {error}")
    {
    }
}
