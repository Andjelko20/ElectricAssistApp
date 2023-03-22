using Server.Models;

namespace Server.Services
{
    /// <summary>
    /// Token Service Dependancy injection
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Create JWT token
        /// </summary>
        /// <param name="user">Logged in user</param>
        /// <returns>JWT token</returns>
        string CreateJwtToken(UserModel user);
    }
}
