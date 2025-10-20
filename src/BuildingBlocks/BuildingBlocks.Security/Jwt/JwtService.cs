using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Ardalis.GuardClauses;
using BuildingBlocks.Core.Utils;
using BuildingBlocks.Security.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace NextGen.Modules.Identity.Shared.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtOptions _jwtOptions;

        public JwtService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions?.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
        }

        /// <summary>
        /// Generate a JWT access token.
        /// - userId is Guid because DB stores user id as uniqueidentifier.
        /// - rolesClaims & permissionsClaims are expected to be loaded from DB by caller
        ///   (join asp_net_user_roles -> asp_net_roles and asp_net_role_claims / asp_net_user_claims).
        /// </summary>
        public string GenerateJwtToken(
            Guid userId,
            string userName,
            string email,
            bool? isVerified = null,
            string? fullName = null,
            string? refreshToken = null,
            IReadOnlyList<Claim>? usersClaims = null,
            IReadOnlyList<string>? rolesClaims = null,
            IReadOnlyList<string>? permissionsClaims = null)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("UserName (unique name) cannot be empty.", nameof(userName));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));

            Guard.Against.NullOrEmpty(_jwtOptions.SecretKey, nameof(_jwtOptions.SecretKey));

            var now = DateTime.UtcNow;
            var ipAddress = IpUtilities.GetIpAddress() ?? string.Empty;

            // Base claims
            var jwtClaims = new List<Claim>
            {
                // Map subject and identifiers to GUID string (DB uses uniqueidentifier)
                new(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new(JwtRegisteredClaimNames.Sid, userId.ToString()),
                new(JwtRegisteredClaimNames.NameId, userId.ToString()),
                new(JwtRegisteredClaimNames.UniqueName, userName),
                new(JwtRegisteredClaimNames.Email, email),
                new(JwtRegisteredClaimNames.Name, fullName ?? string.Empty),
                new(JwtRegisteredClaimNames.Exp,  DateTimeOffset.UtcNow.AddSeconds(_jwtOptions.TokenLifeTimeSecond).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                // issued at as epoch seconds (per JWT best practices)
                new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(CustomClaimTypes.RefreshToken, refreshToken ?? string.Empty),
                new(CustomClaimTypes.IpAddress, ipAddress)
            };

            if (isVerified.HasValue)
            {
                // Optional flag: store as custom claim
                jwtClaims.Add(new Claim(CustomClaimTypes.IsVerified, isVerified.Value ? "true" : "false"));
            }

            // Add roles (as ClaimTypes.Role)
            if (rolesClaims?.Any() == true)
            {
                foreach (var role in rolesClaims.Where(r => !string.IsNullOrWhiteSpace(r)))
                {
                    jwtClaims.Add(new Claim(ClaimTypes.Role, role!.ToLower(CultureInfo.InvariantCulture)));
                }
            }

            // Add permission claims (custom)
            if (permissionsClaims?.Any() == true)
            {
                foreach (var permission in permissionsClaims.Where(p => !string.IsNullOrWhiteSpace(p)))
                {
                    jwtClaims.Add(new Claim(CustomClaimTypes.Permission, permission!.ToLower(CultureInfo.InvariantCulture)));
                }
            }

            // Add any extra user claims already shaped as Claim objects
            if (usersClaims?.Any() == true)
            {
                // Avoid duplicating standard claims - but keep caller's claims
                jwtClaims.AddRange(usersClaims);
            }

            // Optional audience
            if (!string.IsNullOrWhiteSpace(_jwtOptions.Audience))
            {
                jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Aud, _jwtOptions.Audience));
            }

            var secret = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey);
            var signingKey = new SymmetricSecurityKey(secret);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var tokenLifetimeSeconds = _jwtOptions.TokenLifeTimeSecond <= 0 ? 36000 : _jwtOptions.TokenLifeTimeSecond;

            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: string.IsNullOrWhiteSpace(_jwtOptions.Audience) ? null : _jwtOptions.Audience,
                claims: jwtClaims,
                notBefore: now,
                expires: now.AddSeconds(tokenLifetimeSeconds),
                signingCredentials: signingCredentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return token;
        }

        /// <summary>
        /// Validate and get ClaimsPrincipal from token.
        /// Set validateLifetime=true when you need to validate expiry (normal access token validation).
        /// Set validateLifetime=false if you want to extract claims from an expired token (refresh token flow).
        /// </summary>
        public ClaimsPrincipal? GetPrincipalFromToken(string token, bool validateLifetime = false)
        {
            Guard.Against.NullOrEmpty(token, nameof(token));
            Guard.Against.NullOrEmpty(_jwtOptions.SecretKey, nameof(_jwtOptions.SecretKey));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = !string.IsNullOrWhiteSpace(_jwtOptions.Issuer),
                ValidIssuer = _jwtOptions.Issuer,
                ValidateAudience = !string.IsNullOrWhiteSpace(_jwtOptions.Audience),
                ValidAudience = _jwtOptions.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                ValidateLifetime = validateLifetime,
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

                if (validatedToken is not JwtSecurityToken)
                    throw new SecurityTokenException("Invalid token");

                return principal;
            }
            catch (SecurityTokenException)
            {
                // rethrow to caller or return null depending on your policy
                throw;
            }
        }
    }
}
