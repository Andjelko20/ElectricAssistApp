using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs;
using Server.Utilities;
using Server.Models;
using System.Security.Claims;
using Server.Services;
using Server.DTOs.Responses;
using Server.DTOs.Requests;
using Server.Services.Implementations;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        public readonly SqliteDbContext _sqliteDb;
        public readonly ILogger<UsersController> logger;
        public readonly ITokenService tokenService;
        public readonly IUserService userService;
        public readonly IEmailService emailService;
        public readonly int NUMBER_OF_ITEMS_PER_PAGE = 20;

        public UsersController(
            SqliteDbContext sqliteDb,
            ILogger<UsersController> logger,
            ITokenService tokenService,
            IUserService userService,
            IEmailService emailService)
        {
            _sqliteDb = sqliteDb;
            this.logger = logger;
            this.tokenService = tokenService;
            this.userService = userService;
            this.emailService = emailService;
        }
        /// <summary>
        /// Get 20 users per page
        /// </summary>
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataPage<UserDetailsDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestStatusResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MessageResponseDTO), StatusCodes.Status500InternalServerError)]


        [HttpGet]
        [Route("page/{page:int}")]
        [Authorize(Roles = Roles.AdminPermission)]
        public async Task<IActionResult> GetPage([FromRoute]int page)
        {
            try
            {
                return Ok(await userService.GetPageOfUsers(page, 20, (user) => true));
            }
            catch(HttpRequestException ex)
            {
                return StatusCode((int)ex.StatusCode.Value, new MessageResponseDTO(ex.Message));
            }
            catch(Exception ex)
            {
                //logger.LogInformation(ex.Message);
                return StatusCode(500, new {message="Internal server error"});
            }
        }
        /// <summary>
        /// Get single user
        /// </summary>
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserDetailsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestStatusResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MessageResponseDTO), StatusCodes.Status500InternalServerError)]

        [HttpGet]
        [Route("{id:long}")]
        [Authorize(Roles = Roles.AdminPermission)]
        public async Task<IActionResult> GetUserById([FromRoute]long id)
        {
            var user = await userService.GetUserById(id);
            if (user == null)
            {
                return NotFound(new { message="User with id "+id.ToString()+" doesn\'t exist" });
            }
            return Ok(new UserDetailsDTO(user));
        }
        /// <summary>
        /// Get all roles
        /// </summary>
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<RoleModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestStatusResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MessageResponseDTO), StatusCodes.Status500InternalServerError)]


        [HttpGet]
        [Route("roles")]
        [Authorize(Roles = Roles.AdminPermission)]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var id = tokenService.GetClaim(HttpContext,JwtClaims.Id);
                logger.LogInformation(id);
                return Ok(await userService.GetAllRoles());
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
        /// <summary>
        /// Create user
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(MessageResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestStatusResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MessageResponseDTO), StatusCodes.Status500InternalServerError)]

        [HttpPost]
        [Authorize(Roles =Roles.AdminPermission)]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDTO requestBody)
        {
            try
            {
                UserModel user = new UserModel
                {
                    Username = requestBody.Username,
                    Name = requestBody.Name,
                    Password = HashGenerator.Hash(requestBody.Password),
                    Blocked = requestBody.Blocked,
                    RoleId = requestBody.RoleId,
                    Email=requestBody.Email,
                    Address=requestBody.Address,
                    Latitude=requestBody.Latitude,
                    Longitude=requestBody.Longitude,
                    SettlementId=requestBody.SettlementId
                };

                _sqliteDb.Users.Add(user);
                try
                {
                    emailService.SendEmail(requestBody.Email,"Account created","Your account is created successfully. Your password is <b>"+requestBody.Password+"</b>",true);
                }
                catch
                {
                    return StatusCode(500, new MessageResponseDTO("Email is not sent. Check if your email exists."));
                }
                await _sqliteDb.SaveChangesAsync();
                return Ok(new { message="Creted" });
            }
            catch(Exception ex)
            {
                //return StatusCode(400, new { message = "Already exists user with that username" });
                return StatusCode(400, new MessageResponseDTO("Already exists user with that username or email"));
            }

        }
        /// <summary>
        /// Update user by admin
        /// </summary>
        [Produces("application/json")]
        [ProducesResponseType(typeof(MessageResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestStatusResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MessageResponseDTO), StatusCodes.Status500InternalServerError)]


        [HttpPut]
        [Route("{id:long}")]
        [Authorize(Roles =Roles.AdminPermission)]
        public async Task<IActionResult> UpdateUserByAdmin([FromBody] UpdateUserByAdminDTO requestBody, [FromRoute]long id)
        {
            try
            {
                var user = await userService.GetUserById(id);
                if (user == null)
                    return NotFound(new { message = "User doesn't exists" });
                if((requestBody.RoleId==Roles.AdminId && user.RoleId==Roles.DispatcherId) || (requestBody.RoleId == Roles.DispatcherId&& user.RoleId == Roles.AdminId))
                    user.RoleId = requestBody.RoleId;
                user.Blocked = requestBody.Blocked;
                user.Email = requestBody.Email;
                _sqliteDb.Users.Update(user);   
                await _sqliteDb.SaveChangesAsync();
                return Ok(new { message = "User is updated successfully" });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error" });
            }
        }

        /// <summary>
        /// Update logged in user data
        /// </summary>
        [Produces("application/json")]
        [ProducesResponseType(typeof(MessageResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestStatusResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MessageResponseDTO), StatusCodes.Status500InternalServerError)]

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateLoggedInUser([FromBody] UserUpdateDTO requestBody)
        {
            try
            {
                var context = HttpContext.User.Identity as ClaimsIdentity;
                long userId = long.Parse(context.FindFirst(ClaimTypes.Actor).Value);
                var user = await _sqliteDb.Users.FirstOrDefaultAsync(user => user.Id == userId);
                if (user == null)
                    return NotFound(new { message = "User doesn't exists" });
                user.Username = requestBody.Username;
                user.Name = requestBody.Name;
                user.Email = requestBody.Email;
                _sqliteDb.Users.Update(user);
                await _sqliteDb.SaveChangesAsync();
                return Ok(new { message = "User is updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error" });
            }
        }
        /// <summary>
        /// Block or unblock user
        /// </summary>
        [Produces("application/json")]
        [ProducesResponseType(typeof(MessageResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestStatusResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MessageResponseDTO), StatusCodes.Status500InternalServerError)]

        [HttpPut]
        [Route("set_blocked_status/{id:long}")]
        [Authorize(Roles =Roles.AdminPermission)]
        public async Task<IActionResult> BlockUser([FromBody] BlockedStatusDTO requestBody, [FromRoute] long id)
        {
            var user = await _sqliteDb.Users.FirstOrDefaultAsync(user=>user.Id==id);
            if (user==null)
                return NotFound(new { message="User doesn't exists" });
            user.Blocked = requestBody.Status;
            await _sqliteDb.SaveChangesAsync();
            return Ok(new {message="User is blocked successfully"});
        }

        /// <summary>
        /// Delete user
        /// </summary>
        [Produces("application/json")]
        [ProducesResponseType(typeof(MessageResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestStatusResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MessageResponseDTO), StatusCodes.Status500InternalServerError)]

        [HttpDelete]
        [Route("{id:long}")]
        [Authorize(Roles =Roles.AdminPermission)]
        public async Task<IActionResult> DeleteUser([FromRoute] long id)
        {
            var user=await userService.GetUserById(id);
            if (user != null)
            {
                _sqliteDb.Remove(user);
                await _sqliteDb.SaveChangesAsync();
                return Ok(new {message="Deleted user"});
            }

            return NotFound(new { message ="User with id "+id.ToString()+" not found."});
        }
        /// <summary>
        /// Change password
        /// </summary>
        [Produces("application/json")]
        [ProducesResponseType(typeof(MessageResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestStatusResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MessageResponseDTO), StatusCodes.Status500InternalServerError)]

        [HttpPut]
        [Route("change_password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordDTO requestBody)
        {
            long userId = long.Parse(tokenService.GetClaim(HttpContext,"id"));
            UserModel user = await userService.GetUserById(userId);
            if (!HashGenerator.Verify(requestBody.OldPassword, user.Password))
            {
                return BadRequest(new { message = "Old password is not valid" });
            }
            user.Password = HashGenerator.Hash(requestBody.NewPassword);
            await _sqliteDb.SaveChangesAsync();
            return Ok(new { message = "Password changed successfully" });
        }

        [HttpPut]
        [Route("generate_reset_token_admin")]
        [Authorize(Roles =Roles.AdminPermission)]
        public async Task<IActionResult> GenerateResetToken([FromBody] EmailRequestDTO requestBody)
        {
            var user = await userService.GetUserByEmail(requestBody.Email);
            if (user == null)
                return BadRequest(new BadRequestStatusResponse("User not exist"));
            if (user.Role.Name == "superadmin")
                return Forbid();
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
            else if (resetPassword.ExpireAt > DateTime.Now)
                return BadRequest(new MessageResponseDTO("Reset key is already submited on your email"));
            resetPassword.ResetKey = PasswordGenerator.GenerateRandomPassword(10);
            resetPassword.ExpireAt = DateTime.Now.AddMinutes(5);
            try
            {
                emailService.SendEmail(requestBody.Email, "Reset password", "Click on this link to reset your password:<a href='http://localhost:4200/reset-password/" + resetPassword.ResetKey + "'>" + resetPassword.ResetKey + "</a>", true);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new MessageResponseDTO("Email is not sent"));
            }
            if (!exists)
                _sqliteDb.ResetPassword.Add(resetPassword);
            _sqliteDb.SaveChangesAsync();
            return Ok();
        }
    }
}
