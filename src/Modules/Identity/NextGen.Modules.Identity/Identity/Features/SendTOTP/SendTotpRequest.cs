using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextGen.Modules.Identity.Shared.Enum;

namespace NextGen.Modules.Identity.Identity.Features.SendTOTP;
public record SendTotpRequest(DeliveryChannel DeliveryChannel);
