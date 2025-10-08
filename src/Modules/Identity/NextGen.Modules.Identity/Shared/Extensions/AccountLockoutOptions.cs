using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.Modules.Identity.Shared.Extensions;

public class AccountLockoutOptions
{
    /// <summary>
    /// Maximum allowed failed attempts before lockout. Set <= 0 to disable automatic lockout.
    /// Default: 3
    /// </summary>
    public int MaxFailedAccessAttempts { get; set; } = 3;

    /// <summary>
    /// Lockout duration in minutes. Default: 15 minutes.
    /// </summary>
    public int LockoutDurationMinutes { get; set; } = 15;

    /// <summary>
    /// If true, reset failed attempts counter on successful login. Default: true.
    /// </summary>
    public bool ResetFailedCountOnSuccess { get; set; } = true;
}
