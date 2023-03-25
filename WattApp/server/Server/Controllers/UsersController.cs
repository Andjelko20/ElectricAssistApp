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
        public readonly int NUMBER_OF_ITEMS_PER_PAGE = 20;

        public UsersController(
            SqliteDbContext sqliteDb,
            ILogger<UsersController> logger,
            ITokenService tokenService,
            IUserService userService)
        {
            _sqliteDb = sqliteDb;
            this.logger = logger;
            this.tokenService = tokenService;
            this.userService = userService;
        }   
        /// <summary>
        /// Get 20 users per page
        /// </summary>
        [HttpGet]
        [Route("page/{page:int}")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> GetPage([FromRoute]int page)
        {
            try
            {
                /*
                int count = _sqliteDb.Users.Count();
                logger.LogInformation(count.ToString());
                int maxPage = count / NUMBER_OF_ITEMS_PER_PAGE+((count%NUMBER_OF_ITEMS_PER_PAGE!=0)?1:0);
                logger.LogInformation(maxPage.ToString());
                if (page < 1 || page > maxPage)
                    return BadRequest(new { message ="Page number isn\'t valid"});
                var users = await _sqliteDb.Users.Include(u => u.Role).Skip((page - 1) * NUMBER_OF_ITEMS_PER_PAGE).Take(NUMBER_OF_ITEMS_PER_PAGE).Select(u => new
                {
                    Id = u.Id,
                    Name = u.Name,
                    Username = u.Username,
                    Role = u.Role.Name,
                    Blocked = u.Blocked,
                    Email=u.Email

                }).ToListAsync();

                int? previousPage = (page == 1) ? null : page - 1;
                int? nextPage = (page == maxPage) ? null : page + 1;
                return Ok(new {
                    previousPage=previousPage,
                    nextPage=nextPage,
                    numberOfPages=maxPage,
                    data=users
                });
                */
                return Ok(await userService.GetPageOfUsers(page, 20, (user) => true));
            }
            catch(HttpRequestException ex)
            {
                return StatusCode((int)ex.StatusCode.Value, new MessageResponseDTO(ex.Message));
            }
            catch(Exception ex)
            {
                logger.LogInformation(ex.Message);
                return StatusCode(500, new {message="Internal server error"});
            }
        }
        /// <summary>
        /// Get single user
        /// </summary>
        [HttpGet]
        [Route("{id:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUserById([FromRoute]int id)
        {
            var user = await userService.GetUserById(id);
            if (user == null)
            {
                return NotFound(new { message="User with id "+id.ToString()+" doesn\'t exist" });
            }
            return Ok(new
            {
                Name=user.Name,
                Username=user.Username,
                Role=user.Role.Name,
                Blocked=user.Blocked,
                Email=user.Email
            });
        }
        /// <summary>
        /// Get all roles
        /// </summary>
        [HttpGet]
        [Route("roles")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var id = tokenService.GetClaim(HttpContext,JwtClaims.Id);
                logger.LogInformation(id);
                return Ok(await _sqliteDb.Roles.ToListAsync());
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
        [HttpPost]
        [Authorize(Roles ="admin")]
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
                    Email=requestBody.Email
                };

                _sqliteDb.Users.Add(user);
                await _sqliteDb.SaveChangesAsync();
                return Ok(new { message="Creted" });
            }
            catch(Exception ex)
            {
                //return StatusCode(400, new { message = "Already exists user with that username" });
                return StatusCode(400, new MessageResponseDTO("Already exists user with that username"));
            }

        }
        /// <summary>
        /// Update user by admin
        /// </summary>
        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> UpdateUserByAdmin([FromBody] UserUpdateDTO requestBody, [FromRoute]int id)
        {
            try
            {
                var user = await _sqliteDb.Users.FirstOrDefaultAsync(user => user.Id == id);
                if (user == null)
                    return NotFound(new { message = "User doesn't exists" });
                user.Username = requestBody.Username;
                user.Name = requestBody.Name;
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
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateLoggedInUser([FromBody] UserUpdateDTO requestBody)
        {
            try
            {
                var context = HttpContext.User.Identity as ClaimsIdentity;
                int userId = int.Parse(context.FindFirst(ClaimTypes.Actor).Value);
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
        [HttpPut]
        [Route("set_blocked_status/{id:int}")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> BlockUser([FromBody] BlockedStatusDTO requestBody, [FromRoute] int id)
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
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var user=await _sqliteDb.Users.FirstOrDefaultAsync(user=>user.Id==id);
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
        [HttpPut]
        [Route("change_password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordDTO requestBody)
        {
            var credentials = HttpContext.User.Identity as ClaimsIdentity;
            int userId = int.Parse(credentials.FindFirst(ClaimTypes.Actor).Value);
            UserModel user = await _sqliteDb.Users.FirstOrDefaultAsync(user => user.Id == userId);
            if (!HashGenerator.Verify(requestBody.OldPassword, user.Password))
            {
                return BadRequest(new { message = "Old password is not valid" });
            }
            user.Password = HashGenerator.Hash(requestBody.NewPassword);
            await _sqliteDb.SaveChangesAsync();
            return Ok(new { message = "Password changed successfully" });
        }   
    }
}
