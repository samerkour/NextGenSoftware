using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Security.Jwt;
public class RefreshTokenOptions
{
    /// <summary>
    /// How many days the refresh token is valid for (default: 30 days).
    /// </summary>
    public int LifetimeDays { get; set; } = 30;

    /// <summary>
    /// If true, cleanup will soft-revoke old/invalid tokens by setting RevokedAt.
    /// If false, cleanup will physically remove invalid tokens from DB.
    /// </summary>
    public bool SoftRevoke { get; set; } = true;

    /// <summary>
    /// Optional: keep at most this many active tokens per user (rotate/cleanup policy).
    /// Set to 0 to disable this limit.
    /// </summary>
    public int MaxActiveTokensPerUser { get; set; } = 0;
}
