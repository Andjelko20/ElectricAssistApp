using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Controllers
{
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
    }
}
