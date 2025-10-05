using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Core.Exception.Types;

namespace NextGen.Modules.Identity.Users.Features.UserTwoFactorEnable;
internal class UpdateTwoFactorFailedException : AppException
{
    public Guid UserId { get; }
    public bool Enabled { get; }

    public UpdateTwoFactorFailedException(Guid userId, bool enabled)
        : base($"Failed to update TwoFactorEnabled to '{enabled}' for user with Id '{userId}'.")
    {
        UserId = userId;
        Enabled = enabled;
    }
}
