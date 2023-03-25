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
using Server.DTOs.Responses;
using Server.Services.Implementations;

namespace Server.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController:Controller
    {
        public readonly SqliteDbContext _sqliteDb;
        public readonly ITokenService tokenService;
        public readonly ILogger<AuthenticationController> logger;
        public readonly IEmailService emailService;
        public readonly IUserService userService;
        public AuthenticationController(
            SqliteDbContext _sqliteDb,
            ITokenService tokenService,
            ILogger<AuthenticationController> logger,
            IEmailService emailService,
            IUserService userService
            )
        {
            this.tokenService = tokenService;
            this._sqliteDb = _sqliteDb;
            this.logger = logger;
            this.emailService = emailService;
            this.userService = userService;
        }
        /// <summary>Login</summary>
        [Produces("application/json")]
        [ProducesResponseType(typeof(TokenResponseDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestStatusResponse),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MessageResponseDTO),StatusCodes.Status401Unauthorized)]

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginDTO requestBody)
        {
            logger.LogInformation(requestBody.Username);
            var user = await userService.GetUserByUsername(requestBody.Username);
            if (user is null)
            {
                return Unauthorized(new MessageResponseDTO("Bad credentials"));
            }
            if (!HashGenerator.Verify(requestBody.Password, user.Password))
            {
                return Unauthorized(new { message = "Bad credentials" });
            }
            if (user.Blocked)
                return Unauthorized(new { message = "User is blocked" });
            return Ok(new TokenResponseDTO(tokenService.CreateJwtToken(user)));
        }
        /// <summary>
        /// Register as guest
        /// </summary>
        [Produces("application/json")]
        [ProducesResponseType(typeof(MessageResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestStatusResponse), StatusCodes.Status400BadRequest)]

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]UserCreateDTO requestBody)
        {
            RoleModel? role = await _sqliteDb.Roles.FirstOrDefaultAsync(r => r.Name == "guest");
            if (role == null)
            {
                return BadRequest(new BadRequestStatusResponse("You send role which doesn\'t exist"));
            }
            if (_sqliteDb.Users.Any(u => u.Username == requestBody.Username))
            {
                return BadRequest(new BadRequestStatusResponse("User already exists"));
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
            return Ok(new MessageResponseDTO( "Registered successfully" ));
        }

        /// <summary>
        /// Send reset password token on mail
        /// </summary>
        [Produces("application/json")]
        [ProducesResponseType(typeof(MessageResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestStatusResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MessageResponseDTO),StatusCodes.Status500InternalServerError)]


        [HttpPost]
        [Route("generate_reset_token")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateResetToken([FromBody] EmailDTO requestBody)
        {
            var user=await userService.GetUserByEmail(requestBody.Email);
            if (user == null)
                return BadRequest(new BadRequestStatusResponse("User not exist"));
            var resetPassword = await _sqliteDb.ResetPassword.FirstOrDefaultAsync(r => r.UserId == user.Id);
            bool exists = true;
            if (resetPassword == null)
            {
                exists = false;
                resetPassword = new ResetPasswordModel()
                {
                    UserId = user.Id,
                };
            }else if (resetPassword.ExpireAt > DateTime.Now)
                return BadRequest("Reset token is already sent. Check your email.");
            resetPassword.ResetKey = PasswordGenerator.GenerateRandomPassword(10);
            resetPassword.ExpireAt = DateTime.Now.AddMinutes(5);
            try
            {
                emailService.SendEmail(requestBody.Email, "Reset password", "Your code for password reset:<b>" + resetPassword.ResetKey + "</b>", true);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new MessageResponseDTO("Email is not sent. Check if your email exist."));
            }
            if (!exists)
                _sqliteDb.ResetPassword.Add(resetPassword);
            _sqliteDb.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Reset password
        /// </summary>
        [Produces("application/json")]

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
            user.Password = HashGenerator.Hash(requestBody.NewPassword);
            _sqliteDb.ResetPassword.Remove(resetPassword);
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
