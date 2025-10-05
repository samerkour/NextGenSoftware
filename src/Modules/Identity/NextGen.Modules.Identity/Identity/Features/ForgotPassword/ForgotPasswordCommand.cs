using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.Identity.Features.ForgotPassword;
public record ForgotPasswordCommand(string Email) : ITxCreateCommand;
