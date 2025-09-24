using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Core.Exception.Types;

namespace NextGen.Modules.Identity.Identity.Features.SendTOTP;
public class SendTotpException : BadRequestException
{
    public SendTotpException(string error)
        : base($"Failed to send TOTP: {error}")
    {
    }
}
