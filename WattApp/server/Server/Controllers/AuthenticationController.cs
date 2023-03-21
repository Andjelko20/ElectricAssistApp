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
using Server.Services;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController:Controller
    {
        public readonly TokenGenerator tokenGenerator;
        public readonly SqliteDbContext _sqliteDb;
        public readonly ILogger<AuthenticationController> logger;
        public readonly EmailService emailService;
        public AuthenticationController(
            TokenGenerator tokenGenerator,
            SqliteDbContext _sqliteDb, 
            ILogger<AuthenticationController> logger,
            EmailService emailService
            )
        {
            this.tokenGenerator = tokenGenerator;
            this._sqliteDb = _sqliteDb;
            this.logger = logger;
            this.emailService = emailService;
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

        [HttpPost]
        [Route("generate_reset_token")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateResetToken([FromBody] EmailDTO requestBody)
        {
            var user=await _sqliteDb.Users.SingleOrDefaultAsync(u => u.Email == requestBody.Email);
            if (user == null)
                return BadRequest(new Message("User not exist"));
            var resetPassword = await _sqliteDb.ResetPassword.FirstOrDefaultAsync(r => r.UserId == user.Id);
            bool exists = true;
            if (resetPassword == null)
            {
                exists = false;
                resetPassword = new ResetPasswordModel()
                {
                    UserId = user.Id,
                };
            }
            resetPassword.ResetKey = CreatePassword(10);
            resetPassword.ExpireAt = DateTime.Now.AddMinutes(5);
            emailService.SendEmail(requestBody.Email, "Reset password", "Your code for password reset:<b>"+resetPassword.ResetKey+"</b>",true);
            if (!exists)
                _sqliteDb.ResetPassword.Add(resetPassword);
            _sqliteDb.SaveChangesAsync();
            return Ok();
        }

        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        [HttpPost]
        [Route("reset_password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO requestBody)
        {
            _sqliteDb.ResetPassword.RemoveRange(_sqliteDb.ResetPassword.Where(r=>r.ExpireAt<DateTime.Now));
            var resetPassword=await _sqliteDb.ResetPassword.FirstOrDefaultAsync(r=>r.ResetKey==requestBody.ResetKey);
            if (resetPassword == null)
                return BadRequest();
            var user = await _sqliteDb.Users.FirstOrDefaultAsync(u => u.Id == resetPassword.UserId);
            if (user == null)
                return BadRequest();
            user.Password = requestBody.NewPassword;
            _sqliteDb.SaveChangesAsync();
            return Ok();
        }
    }
    public class EmailDTO
    {
        [Required]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Not email")]
        public string Email { get; set; }
    }

    public class ResetPasswordDTO
    {
        [Required]

        public string ResetKey { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
