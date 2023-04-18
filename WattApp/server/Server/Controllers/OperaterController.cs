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
    public class OperaterController : Controller
    {
        public readonly SqliteDbContext _sqliteDb;
        public readonly ILogger<UsersController> logger;
        public readonly ITokenService tokenService;
        public readonly IUserService userService;
        public readonly IEmailService emailService;
        public readonly int NUMBER_OF_ITEMS_PER_PAGE = 20;

        public OperaterController(
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
        [Authorize(Roles = Roles.OperaterPermission)]
        public async Task<IActionResult> GetPage([FromRoute]int page)
        {
            try
            {
                return Ok(await userService.GetPageOfUsers(page, 20, (user) => user.RoleId==Roles.ProsumerId));
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
        [Authorize(Roles = Roles.OperaterPermission)]
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
        /// Create user
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad request</response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(MessageResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestStatusResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MessageResponseDTO), StatusCodes.Status500InternalServerError)]

        [HttpPost]
        [Authorize(Roles =Roles.OperaterPermission)]
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
                    RoleId = Roles.ProsumerId,
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
            if (user != null && user.RoleId==Roles.ProsumerId)
            {
                _sqliteDb.Remove(user);
                await _sqliteDb.SaveChangesAsync();
                return Ok(new {message="Deleted user"});
            }

            return NotFound(new { message ="User with id "+id.ToString()+" not found."});
        }
    }
}
