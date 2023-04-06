using System.Security.Claims;

namespace Server.Utilities
{
    /// <summary>
    /// JwtClaims
    /// </summary>
    public class JwtClaims
    {
        /// <summary>
        /// Id
        /// </summary>
        public static readonly string Id = "id";

        /// <summary>
        /// Role
        /// </summary>
        public static readonly string Role = ClaimTypes.Role;
    }
}
