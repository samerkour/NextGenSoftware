using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Abstractions.CQRS.Command;

namespace NextGen.Modules.Identity.Users.Features.ChangeUserPassword;

public record ChangeUserPasswordCommand(
    Guid UserId,
    string CurrentPassword,
    string NewPassword,
    string ConfirmNewPassword
) : ITxUpdateCommand;
