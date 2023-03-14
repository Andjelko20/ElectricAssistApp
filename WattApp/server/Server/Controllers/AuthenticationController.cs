using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs;
using Server.Utilities;
using Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Server.Middlewares;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController:Controller
    {
        public readonly TokenGenerator tokenGenerator;
        public readonly SqliteDbContext _sqliteDb;
        public readonly ILogger<AuthenticationController> logger;
        public AuthenticationController(TokenGenerator tokenGenerator,SqliteDbContext _sqliteDb, ILogger<AuthenticationController> logger)
        {
            this.tokenGenerator = tokenGenerator;
            this._sqliteDb = _sqliteDb;
            this.logger = logger;
        }
        /// <summary>Login</summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginDTO requestBody)
        {
            UserModel? user = await _sqliteDb.Users.Include(u=>u.Role).FirstOrDefaultAsync(user => user.Username == requestBody.Username);
            if (user == null)
            {
                return Unauthorized(new { message = "Bad credentials" });
            }
            if (!HashGenerator.Verify(requestBody.Password, user.Password))
            {
                return Unauthorized(new { message = "Bad credentials" });
            }
            if (user.Blocked)
                return Unauthorized(new { message = "User is blocked" });
            return Ok(new { token = tokenGenerator.GenerateJwtToken(user) });
        }
        /// <summary>
        /// Register as guest
        /// </summary>
        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]UserCreateDTO requestBody)
        {
            RoleModel? role = await _sqliteDb.Roles.FirstOrDefaultAsync(r => r.Name == "guest");
            if (role == null)
            {
                return StatusCode(500,new {message="Internal Server Error"});
            }
            if (_sqliteDb.Users.Any(u => u.Username == requestBody.Username))
            {
                return BadRequest(new {message="User already exists"});
            }
            UserModel user = new UserModel
            {
                Name= requestBody.Name,
                Username=requestBody.Username,
                Password=HashGenerator.Hash(requestBody.Password),
                RoleId=role.Id,
                Blocked=false
            };
            await _sqliteDb.Users.AddAsync(user);
            await _sqliteDb.SaveChangesAsync();
            return Ok(new { message = "Registered successfully" });
        }
    }
}
