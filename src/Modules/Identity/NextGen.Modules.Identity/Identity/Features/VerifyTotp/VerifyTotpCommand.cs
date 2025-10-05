using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Abstractions.CQRS.Command;
using NextGen.Modules.Identity.Shared.Enum;

namespace NextGen.Modules.Identity.Identity.Features.VerifyTotp;
public record VerifyTotpCommand(Guid UserId, string Code, DeliveryChannel DeliveryChannel) : ITxCreateCommand;
