using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Server.Models;
using Server.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Services.Implementations
{
    /// <summary>
    /// Create token and get data from token
    /// </summary>
    public class TokenService:ITokenService
    {
        /// <summary>
        /// Represents a set of key/value application configuration properties
        /// </summary>
        public readonly IConfiguration config;

        /// <summary>
        /// Dependency injection
        /// </summary>
        /// <param name="config"></param>
        public TokenService(IConfiguration config)
        {
            this.config = config;
        }

        /// <inheritdoc/>
        public string CreateJwtToken(UserModel user)
        {
            var key = Encoding.ASCII.GetBytes(config.GetSection("JwtConfig:SecretKey").Value);//jwtConfig.SecretKey);
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptior = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Actor, user.Id.ToString()),
                    new Claim(JwtClaims.Id, user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.Role.Name)
                }),
                Expires = DateTime.Now.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptior);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
        /// <inheritdoc/>
        public string? GetClaim(HttpContext httpContext,string index)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            return identity?.FindFirst(index)?.Value;
        }

    }
}
