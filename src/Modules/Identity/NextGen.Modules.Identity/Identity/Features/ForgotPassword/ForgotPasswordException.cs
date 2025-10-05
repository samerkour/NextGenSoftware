using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Core.Exception.Types;

namespace NextGen.Modules.Identity.Identity.Features.ForgotPassword;

public class ForgotPasswordException : BadRequestException
{
    public ForgotPasswordException(string error)
        : base($"Forgot password failed: {error}")
    {
    }
}
