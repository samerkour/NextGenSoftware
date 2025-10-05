using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Core.Exception.Types;
using NextGen.Modules.Identity.Shared.Enum;

namespace NextGen.Modules.Identity.Identity.Features.VerifyTotp;
internal class InvalidTotpException : AppException
{
    public Guid UserId { get; }
    public DeliveryChannel Channel { get; }

    public InvalidTotpException(Guid userId, DeliveryChannel channel)
        : base($"Invalid TOTP code for user '{userId}' via {channel}.")
    {
        UserId = userId;
        Channel = channel;
    }
}
