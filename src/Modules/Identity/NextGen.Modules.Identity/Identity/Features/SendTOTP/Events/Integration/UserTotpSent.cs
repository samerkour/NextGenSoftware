using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.Modules.Identity.Identity.Features.SendTOTP.Events.Integration;
public record UserTotpSent(
    Guid UserId,
    string? Email,
    string? PhoneNumber,
    string DeliveryChannel,
    string Token
);
