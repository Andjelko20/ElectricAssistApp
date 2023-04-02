﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Server.Data;
using Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Server.Middlewares
{
    /// <summary>
    /// Token validation middleware
    /// </summary>
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Dependency injection
        /// </summary>
        /// <param name="next"></param>
        /// <param name="configuration"></param>
        public TokenValidationMiddleware(
            RequestDelegate next, 
            IConfiguration configuration
        )
        {
            _next = next;
            _configuration = configuration;
        }

        /// <summary>
        /// Check if role in token is the same as role in database
        /// </summary>
        /// <param name="context"></param>
        /// <param name="_sqliteDb"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context, SqliteDbContext _sqliteDb)
        {
            var token = context.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_configuration["JwtConfig:SecretKey"]);
                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                    var claims = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                    var userIdClaim = claims.FindFirst(c => c.Type == ClaimTypes.Actor);
                    var userRoleClaim = claims.FindFirst(c => c.Type == ClaimTypes.Role);

                    if (userIdClaim == null || userRoleClaim == null)
                    {
                        throw new Exception();
                    }

                    var userId = long.Parse(userIdClaim.Value);
                    var userRole = userRoleClaim.Value;

                    // Check if user with the given ID has the required role
                    // You will need to implement your own logic here
                    if (!HasUserRole(_sqliteDb,userId, userRole))
                    {
                        throw new Exception();
                    }
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = "Bad token" }));
                    return;
                }
            }

            await _next(context);
        }

        private static bool HasUserRole(SqliteDbContext _sqliteDb,long userId, string userRole)
        {
            RoleModel role = _sqliteDb.Roles.FirstOrDefault(role => role.Name == userRole);
            if (role == null)
                return false;
            long roleId = role.Id;
            return _sqliteDb.Users.Any(user=>user.Id==userId && user.RoleId==roleId);
        }
    }
    /// <summary>
    /// Static class for using middleware
    /// </summary>
    public static class TokenValidationMiddlewareExtensions
    {
        /// <summary>
        /// Allow to use app.UseTokenValidation() as method of app
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseTokenValidation(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenValidationMiddleware>();
        }
    }
}
