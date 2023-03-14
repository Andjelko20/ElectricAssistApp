using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs;
using Server.Utilities;
using Server.Models;
using System.Security.Claims;

namespace Server.Controllers
{
    public class Message
    {
        public string message;
        public Message(string message)
        {
            this.message = message;
        }
    }


    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        public readonly ILogger<UsersController> logger;
        public readonly SqliteDbContext _sqliteDb;
        public readonly int NUMBER_OF_ITEMS_PER_PAGE = 20;

        public UsersController(SqliteDbContext sqliteDb,ILogger<UsersController> logger)
        {
            _sqliteDb = sqliteDb;
            this.logger = logger;
        }   

        [HttpGet]
        [Route("page/{page:int}")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> GetPage([FromRoute]int page)
        {
            try
            {
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
                    Blocked = u.Blocked

                }).ToListAsync();

                int? previousPage = (page == 1) ? null : page - 1;
                int? nextPage = (page == maxPage) ? null : page + 1;
                return Ok(new {
                    previousPage=previousPage,
                    nextPage=nextPage,
                    numberOfPages=maxPage,
                    data=users
                });
            }
            catch(Exception e)
            {
                return StatusCode(500, new {message="Internal server error"});
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> GetUserById([FromRoute]int id)
        {
            var user = await _sqliteDb.Users.Include(u=>u.Role).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { message="User with id "+id.ToString()+" doesn\'t exist" });
            }
            return Ok(new
            {
                Name=user.Name,
                Username=user.Username,
                Role=user.Role.Name,
                Blocked=user.Blocked
            });
        }

        [HttpGet]
        [Route("roles")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                return Ok(await _sqliteDb.Roles.ToListAsync());
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
        /// <summary>
        /// 
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
                    RoleId = requestBody.RoleId
                };

                _sqliteDb.Users.Add(user);
                await _sqliteDb.SaveChangesAsync();
                return Ok(new { message="Creted" });
            }
            catch(Exception ex)
            {
                //return StatusCode(400, new { message = "Already exists user with that username" });
                return StatusCode(400, new Message("Already exists user with that username"));
            }

        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDTO requestBody, [FromRoute]int id)
        {
            try
            {
                var context = HttpContext.User.Identity as ClaimsIdentity;
                int userId = int.Parse(context.FindFirst(ClaimTypes.Actor).Value);
                string role = context.FindFirst(ClaimTypes.Role).Value;
                if (role != "admin" && userId != id)
                {
                    var dict = new Dictionary<string, string?>();
                    dict.Add("message", "Forbidden");
                    return Forbid(new AuthenticationProperties());
                }
                var user = await _sqliteDb.Users.FirstOrDefaultAsync(user => user.Id == id);
                if (user == null)
                    return NotFound(new { message = "User doesn't exists" });
                user.Username = requestBody.Username;
                user.Name = requestBody.Name;
                if (role == "admin" && requestBody.RoleId > 0)
                {
                    user.RoleId = requestBody.RoleId;
                    user.Blocked = requestBody.Blocked;
                }
                _sqliteDb.Users.Update(user);   
                await _sqliteDb.SaveChangesAsync();
                return Ok(new { message = "User is updated successfully" });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error" });
            }
        }

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
    }
}
