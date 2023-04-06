using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs;
using Server.DTOs.Responses;
using Server.Services;
using Server.Services.Implementations;
using Server.Utilities;

namespace Server.Controllers
{
    [ApiController]
    public class ProsumersController : Controller
    {
        public readonly IUserService userService;

        public ProsumersController(IUserService userService)
        {
            this.userService = userService;
        }

        ///<summary>Get all prosumers for map</summary>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestStatusResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MessageResponseDTO), StatusCodes.Status500InternalServerError)]

        [HttpGet]
        [Route("")]
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
    }
}
