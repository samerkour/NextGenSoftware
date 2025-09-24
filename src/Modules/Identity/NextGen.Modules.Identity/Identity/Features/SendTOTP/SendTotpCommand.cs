using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Abstractions.CQRS.Command;
using NextGen.Modules.Identity.Shared.Enum;

namespace NextGen.Modules.Identity.Identity.Features.SendTOTP;
public record SendTotpCommand(
    Guid UserId,
    DeliveryChannel DeliveryChannel // "Email" or "Phone"
) : ITxCreateCommand;
