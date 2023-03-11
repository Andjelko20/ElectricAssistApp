using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Server.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Server.Middlewares
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly SqliteDbContext _sqliteDb;

        public TokenValidationMiddleware(RequestDelegate next, IConfiguration configuration,SqliteDbContext sqliteDb)
        {
            _next = next;
            _configuration = configuration;
            _sqliteDb = sqliteDb;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);
                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                    var claims = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                    var userIdClaim = claims.FindFirst(c => c.Type == "id");
                    var userRoleClaim = claims.FindFirst(c => c.Type == "role");

                    if (userIdClaim == null || userRoleClaim == null)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Bad token");
                        return;
                    }

                    var userId = int.Parse(userIdClaim.Value);
                    var userRole = userRoleClaim.Value;

                    // Check if user with the given ID has the required role
                    // You will need to implement your own logic here
                    if (!HasUserRole(userId, userRole))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Bad token");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Bad token");
                    return;
                }
            }

            await _next(context);
        }

        private bool HasUserRole(int userId, string userRole)
        {
            return true;
        }
    }
}
