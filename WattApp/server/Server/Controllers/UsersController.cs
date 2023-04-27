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
using MimeKit.Encodings;
using Server.Exceptions;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Asn1.Ocsp;

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
        public readonly IConfiguration configuration;
        public readonly int NUMBER_OF_ITEMS_PER_PAGE = 20;

        public UsersController(
            SqliteDbContext sqliteDb,
            ILogger<UsersController> logger,
            ITokenService tokenService,
            IUserService userService,
            IEmailService emailService,
            IConfiguration configuration)
        {
            _sqliteDb = sqliteDb;
            this.logger = logger;
            this.tokenService = tokenService;
            this.userService = userService;
            this.emailService = emailService;
            this.configuration = configuration;
        }
        /// <summary>
        /// Get 20 users per page
        /// </summary>
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataPage<UserDetailsDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestStatusResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MessageResponseDTO), StatusCodes.Status500InternalServerError)]


        [HttpGet]
        [Route("page")]
        [Authorize(Roles = Roles.AdminOperaterPermission)]
        public async Task<IActionResult> GetPage([FromQuery]int pageNumber, [FromQuery] int pageSize=20)
        {
            try
            {
                long myId = long.Parse(tokenService.GetClaim(HttpContext,"id"));
                if (User.IsInRole(Roles.Operater))
                    return Ok(await userService.GetPageOfUsers(pageNumber, pageSize, (user) => user.RoleId==Roles.ProsumerId && user.RoleId!=Roles.SuperadminId && user.Id!=myId));
                return Ok(await userService.GetPageOfUsers(pageNumber, pageSize, (user) => user.RoleId != Roles.SuperadminId && user.Id!=myId));
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
        [Authorize(Roles = Roles.AdminOperaterPermission)]
        public async Task<IActionResult> GetUserById([FromRoute]long id)
        {
            var user = await userService.GetUserById(id);
            if (user == null)
            {
                return NotFound(new { message="User with id "+id.ToString()+" doesn\'t exist" });
            }
            if (user.RoleId == Roles.SuperadminId)
                return Forbid();
            if (User.IsInRole(Roles.Operater) && user.RoleId != Roles.ProsumerId)
                return Forbid();
            return Ok(new UserDetailsDTO(user));
        }

        /// <summary>
        /// Get data for logged in user
        /// </summary>
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserDetailsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestStatusResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MessageResponseDTO), StatusCodes.Status500InternalServerError)]

        [HttpGet]
        [Route("my_data")]
        [Authorize]
        public async Task<IActionResult> GetMyData()
        {
            string iid = tokenService.GetClaim(HttpContext, "id");
            var id = long.Parse(iid);
            var user = await userService.GetUserById(id);
            if (user == null)
            {
                return NotFound(new { message = "User with id " + id.ToString() + " doesn\'t exist" });
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
        [Authorize(Roles = Roles.AdminOperaterPermission)]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var id = tokenService.GetClaim(HttpContext,JwtClaims.Id);
                //logger.LogInformation(id);
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
        [Authorize(Roles = Roles.AdminPermission)]
        public IActionResult CreateUser([FromBody] UserCreateDTO requestBody)
        {
            try
            {
                string password = PasswordGenerator.GenerateRandomPassword();
                PendingUserModel user = new PendingUserModel
                {
                    Username = requestBody.Username,
                    Name = requestBody.Name,
                    Password = HashGenerator.Hash(password),
                    Blocked = requestBody.Blocked,
                    RoleId = requestBody.RoleId,
                    Email = requestBody.Email,
                    Address = requestBody.Address,
                    Latitude = requestBody.Latitude,
                    Longitude = requestBody.Longitude,
                    SettlementId = requestBody.SettlementId, 
                    ExpireAt = DateTime.Now.AddDays(1),
                    ConfirmKey = PasswordGenerator.GenerateRandomPassword(15)//ConfirmEmailKeyGenerator.GenerateConfirmEmailKey()
                };

                var pendingUser = userService.CreatePendingUser(user);
                if (pendingUser == null)
                    throw new EmailAddressAlreadyInUseException("Doslo je do greske prilikom kreiranja zahteva.");
                else if(pendingUser is HttpRequestException)
                {
                    throw (HttpRequestException)pendingUser;
                }
                
                try
                {
                    emailService.SendEmail(user.Email,
                            "Confirm Your Email Address",
                             "Hello " + user.Name + ", <br><br>" +
                            "Thank you for signing up for our service.<br>" +
                            "Before you can start using your account," +
                            "we need to verify your email address.<br> <br>" +
                            "Please click the link below to <b>confirm your email address</b>:<br>" +
                            "<a href='" + configuration.GetValue<string>("frontUrl") + "/email-confirmation?key=" + user.ConfirmKey + "'>" + user.Email + "</a><br><br>" +
                            "If you did not sign up for our service, please ignore this email.<br><br>" +
                            "Thank you, <br>" +
                            "<i><b>ElectricAssist Team</b></i>"
                        , true);
                }
                catch
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new MessageResponseDTO("Email is not sent"));
                }


                return Ok("Email sent successfully!");
               
            }
            catch(HttpRequestException ex)
            {
                return StatusCode(405, ex.Message);
            }
            catch(EmailAddressAlreadyInUseException ex)
            {
                return StatusCode(500, "Ooops... Something went wrong, please try again.");
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Ooops... Something went wrong, please try again.");
            }

        }

        [HttpPost("emailConfirmation/{key}")]
        public IActionResult ConfirmEmailAddress([FromRoute] string key)
        {
            object response = userService.ConfirmEmailAddress(key);
            ConfirmEmailResponseDTO responseDTO = new ConfirmEmailResponseDTO();
            if (response is HttpRequestException)
            {
                responseDTO.error = ((HttpRequestException)response).Message;
            }
            else
            {
                UserModel user = (UserModel)response;
                string password = PasswordGenerator.GenerateRandomPassword(15);
                try
                {
                    emailService.SendEmail(user.Email,
                        "ElecticAssist account created successfully - details",
                        "Hello " + user.Name + ",<br><br>"
                        + "Congratulations! You have successfully created an account and can now start using our app.<br>" +
                        "Please find your login details below:"
                        + "<br><br>" +
                        "Username: " + "<b>" + user.Username + "</b><br>" +
                        "Password: " + "<b>" + password + "</b>" +
                        "<br><br>" +
                        "For security reasons, we recommend that you change your password by logging into your account and accessing the account settings. This will help to protect your account.<br>" +
                        "<br> Thank you, <br> <b><i>ElecticAssist Team</b></i>",
                        true);
                }
                catch
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new MessageResponseDTO("Email is not sent"));
                }
                user.Password = HashGenerator.Hash(password);
                _sqliteDb.Users.Update(user);
                _sqliteDb.SaveChanges();
                responseDTO.isConfirmed = true;
            }
            return Ok(responseDTO);
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
                if (user.RoleId == Roles.SuperadminId)
                    return Forbid();
                if ((requestBody.RoleId==Roles.AdminId && user.RoleId==Roles.DispatcherId) || (requestBody.RoleId == Roles.DispatcherId&& user.RoleId == Roles.AdminId))
                    user.RoleId = requestBody.RoleId;
                user.Blocked = requestBody.Blocked;
                if (user.Email != requestBody.Email)
                {
                    ChangeEmailModel changeEmailModel = new ChangeEmailModel
                    {
                        UserId = id,
                        OldEmail = user.Email,
                        NewEmail = requestBody.Email,
                        ExpireAt = DateTime.Now.AddDays(1),
                        ChangeEmailKey = PasswordGenerator.GenerateRandomPassword(15)//ChangeEmailConfirmationKeyGenerator.GenerateConfirmEmailKey()
                    };

                    userService.CreateChangeEmailRequest(changeEmailModel);

                    try
                    {
                        emailService.SendEmail(changeEmailModel.NewEmail,
                                "Confirm Your Email Address Change",
                                 "Hello " + user.Name + ", <br><br>" +
                                "Thank you for using our service.<br>" +
                                "We have received a request to change the email address associated with your account." +
                                "<br>To complete this process, please confirm the change by clicking on the link below:<br><br>" +
                                "<a href='" + configuration.GetValue<string>("frontUrl") + "/change-email-confirmation?key=" + changeEmailModel.ChangeEmailKey + "'>" + changeEmailModel.NewEmail + "</a><br><br>" +
                                "If you did not initiate this email address change request, please contact" +
                                "our administrator immediately so we can investigate and take appropriate action to protect your account.<br><br>" +

                                "Thank you, <br>" +
                                "<i><b>ElectricAssist Team</b></i>"
                            , true);

                    }
                    catch
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new MessageResponseDTO("Email is not sent"));
                    }
                }
                _sqliteDb.Users.Update(user);   
                await _sqliteDb.SaveChangesAsync();
                return Ok(new { message = "User is updated successfully" });
                //Dobijam sifru i nov mail, ako je sifra dobra menjam mail za novog korisnika
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
                if (user.Email  != requestBody.Email)
                {
                    //Znaci da zeli da menja email
                    ChangeEmailModel changeEmailModel = new ChangeEmailModel
                    {
                        UserId = userId,
                        OldEmail = user.Email,
                        NewEmail = requestBody.Email,
                        ExpireAt = DateTime.Now.AddDays(1),
                        ChangeEmailKey = PasswordGenerator.GenerateRandomPassword(15)//ChangeEmailConfirmationKeyGenerator.GenerateConfirmEmailKey()
                    };

                    userService.CreateChangeEmailRequest(changeEmailModel);

                    try
                    {
                        emailService.SendEmail(changeEmailModel.NewEmail,
                                "Confirm Your Email Address Change",
                                 "Hello " + user.Name + ", <br><br>" +
                                "Thank you for using our service.<br>" +
                                "We have received a request to change the email address associated with your account." +
                                "<br>To complete this process, please confirm the change by clicking on the link below:<br><br>" +
                                "<a href='" + configuration.GetValue<string>("frontUrl") + "/change-email-confirmation?key=" + changeEmailModel.ChangeEmailKey + "'>" + changeEmailModel.NewEmail + "</a><br><br>" +
                                "If you did not initiate this email address change request, please contact" +
                                "our administrator immediately so we can investigate and take appropriate action to protect your account.<br><br>" +

                                "Thank you, <br>" +
                                "<i><b>ElectricAssist Team</b></i>"
                            , true);
                        //FrontUrl, ClientUrl

                    }
                    catch
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new MessageResponseDTO("Email is not sent"));
                    }
                }
                _sqliteDb.Users.Update(user);
                await _sqliteDb.SaveChangesAsync();
                return Ok(new { message = "User is updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error" });
            }
        }

        [HttpPost("changeEmailConfirmation/{key}")]
        public IActionResult changeEmailAddressConfirmation([FromRoute]string key)
        {
            object response = userService.ConfirmChageOfEmailAddress(key);
            ConfirmEmailResponseDTO responseDTO = new ConfirmEmailResponseDTO();
            if (response is HttpRequestException)
            {
                responseDTO.error = ((HttpRequestException)response).Message;
            }
            else
            {
                responseDTO.isConfirmed = true;
            }
            return Ok(responseDTO);
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
            long userId = long.Parse(tokenService.GetClaim(HttpContext, "id"));
            if (userId == id)
            {
                return NotFound(new { message = "User doesn't exists" });
            }
            var user = await _sqliteDb.Users.FirstOrDefaultAsync(user=>user.Id==id);
            if (user==null)
                return NotFound(new { message = "User doesn't exists" });
            if (user.RoleId == Roles.SuperadminId)
                return Forbid();
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
                if (user.RoleId == Roles.SuperadminId)
                    return Forbid();
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
            resetPassword.ResetKey = PasswordGenerator.GenerateRandomPassword(15);
            resetPassword.ExpireAt = DateTime.Now.AddMinutes(5);
            try
            {
                emailService.SendEmail(requestBody.Email, "Reset password", "Click on this link to reset your password:<a href='"+ configuration.GetValue<string>("frontUrl") + "/reset-password/" + resetPassword.ResetKey + "'>" + resetPassword.ResetKey + "</a>", true);
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
