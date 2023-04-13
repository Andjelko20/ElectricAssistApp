using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs;
using Server.DTOs.Responses;
using Server.Services;
using Server.Services.Implementations;
using Server.Utilities;
using SQLitePCL;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProsumersDetailsController : Controller
    {
        public readonly IUserService userService;
        public readonly SqliteDbContext _sqliteDb;
        public readonly IProsumerService prosumerService;

        public ProsumersDetailsController(IUserService userService,SqliteDbContext sqliteDb,IProsumerService prosumerService)
        {
            this.userService = userService;
            _sqliteDb = sqliteDb;
            this.prosumerService = prosumerService;
        }

        ///<summary>Get all prosumers for map</summary>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestStatusResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MessageResponseDTO), StatusCodes.Status500InternalServerError)]

        [HttpGet]
        [Authorize(Roles = Roles.Dispatcher)]
        public async Task<IActionResult> GetAllProsumers()
        {
            return Ok(await userService.GetAllProsumers());
        }

        /// <summary>
        /// Get page of prosumers
        /// </summary>
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataPage<UserDetailsDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestStatusResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MessageResponseDTO), StatusCodes.Status500InternalServerError)]

        [HttpGet]
        [Route("page/{page:int}")]
        [Authorize(Roles=Roles.Dispatcher)]
        public async Task<IActionResult> GetPage([FromRoute] int page)
        {
            try
            {
                return Ok(await userService.GetPageOfUsers(page, 20, (user) => user.RoleId==Roles.ProsumerId));
            }
            catch (HttpRequestException ex)
            {
                return StatusCode((int)ex.StatusCode.Value, new MessageResponseDTO(ex.Message));
            }
            catch (Exception ex)
            {
                //logger.LogInformation(ex.Message);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet]
        [Route("{id:long}")]
        [Authorize(Roles = Roles.Dispatcher)]
        public async Task<IActionResult> GetUserById([FromRoute] long id)
        {
            var user = await userService.GetUserById(id);
            if (user == null)
            {
                return NotFound(new { message = "User with id " + id.ToString() + " doesn\'t exist" });
            }
            if (user.RoleId != Roles.ProsumerId)
                return Forbid();
            return Ok(new UserDetailsDTO(user));
        }

        [HttpGet]
        [Route("count")]
        [Authorize(Roles = Roles.Dispatcher)]
        public async Task<IActionResult> GetCount([FromRoute] long id)
        {
            return Ok(_sqliteDb.Users.ToList().Count(user => user.RoleId == Roles.ProsumerId));
        }
    }
}
