using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTO;
using Server.Helpers;
using Server.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController:Controller
    {
        public readonly TokenGenerator tokenGenerator;
        public readonly SqliteDbContext userContext;
        public AuthenticationController(TokenGenerator tokenGenerator,SqliteDbContext userDbContext)
        {
            this.tokenGenerator=tokenGenerator;
            this.userContext=userDbContext;
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginDTO requestBody)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x=>x.Errors).Select(y=>y.ErrorMessage).ToList());
            }
            UserModel? user = await userContext.Users.FirstOrDefaultAsync(user => user.Username == requestBody.Username);
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

        [HttpGet]
        [Authorize]
        public IActionResult GetAuthorized()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            return Ok(new { message = "You are authorized", id = identity.FindFirst("Id").Value });
        }
    }
}
