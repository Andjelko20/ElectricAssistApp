using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Helpers
{
    public class TokenGenerator
    {
        public readonly IConfiguration config;
        public TokenGenerator(IConfiguration config)
        {
            this.config = config;
        }
        public string GenerateJwtToken(UserModel user)
        {
            var key = Encoding.ASCII.GetBytes(config.GetSection("JwtConfig:SecretKey").Value);//jwtConfig.SecretKey);
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            
            var tokenDescriptior = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Actor, user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.Role.Name)
                }),
                Expires = DateTime.Now.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptior);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }

    }
}
