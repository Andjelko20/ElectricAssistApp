using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs;
using Server.DTOs.Responses;
using Server.Models;
using Server.Models.DropDowns.Devices;
using Server.Models.DropDowns.Location;
using Server.Utilities;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;

namespace Server.Services.Implementations
{
    public class UserService : IUserService
    {
        public readonly SqliteDbContext context;
        public readonly ILogger<UserService> logger;
        public readonly IHistoryService historyService;
        public UserService(SqliteDbContext context, ILogger<UserService> logger, IHistoryService historyService)
        {
            this.context = context;
            this.logger = logger;
            this.historyService = historyService;
        }
        public int GetNumberOfPages(int itemsPerPage, Func<UserModel, bool> filter)
        {
            int numberOfItems = context.Users.Count(filter);
            if (numberOfItems % itemsPerPage == 0)
                return numberOfItems / itemsPerPage;
            return numberOfItems / itemsPerPage + 1;
        }
        public async Task<DataPage<UserDetailsDTO>> GetPageOfUsers(int pageNumber, int itemsPerPage, Func<UserModel, bool> filter)
        {
            DataPage<UserDetailsDTO> page = new();
            page.NumberOfPages = GetNumberOfPages(itemsPerPage, filter);
            if (page.NumberOfPages == 0)
                throw new HttpRequestException("No items", null, HttpStatusCode.NotFound);
            if (page.NumberOfPages < pageNumber || pageNumber < 1)
                throw new HttpRequestException("Invalid page number", null, HttpStatusCode.BadRequest);
            logger.LogInformation(page.NumberOfPages.ToString());
            var users = context.Users
                .Include(user => user.Role)
                .Include(user => user.Settlement)
                .Include(user => user.Settlement.City)
                .Include(user => user.Settlement.City.Country)
                .Where(filter)
                .Skip((pageNumber - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .Select(user => new UserDetailsDTO(user))
                .ToList();
            page.Data = users;
            page.PreviousPage = (pageNumber == 1) ? null : pageNumber - 1;
            page.NextPage = (pageNumber == page.NumberOfPages) ? null : pageNumber + 1;
            return page;
        }

        public Task<UserModel?> GetUserByEmail(string email)
        {
            return context
                .Users
                .Include(user => user.Role)
                .Include(user => user.Settlement)
                .Include(user => user.Settlement.City)
                .Include(user => user.Settlement.City.Country)
                .FirstOrDefaultAsync(user => user.Email == email);
        }
        public async Task<UserModel?> GetUserByUsername(string username)
        {
            try
            {
                var user = await context.Users.Include(user => user.Role).FirstOrDefaultAsync(user => user.Username == username);
                return user;
            }
            catch (NullReferenceException ex)
            {
                return null;
            }
        }
        public Task<UserModel?> GetUserById(long id)
        {
            return context
                .Users
                .Include(user => user.Role)
                .Include(user => user.Settlement)
                .Include(user => user.Settlement.City)
                .Include(user => user.Settlement.City.Country)
                .FirstOrDefaultAsync(user => user.Id == id);
        }

        public Task<List<RoleModel>> GetAllRoles()
        {
            return context.Roles.ToListAsync();
        }

        public Task<UserModel?>? Login(string username)
        {
            return context.Users.Include(user => user.Role).FirstOrDefaultAsync(user => user.Username == username || user.Email == username);
        }

        public async Task<List<object>> GetAllProsumers()
        {
            List<UserModel> allUsers = await context
                .Users
                .Where(user => user.RoleId == Roles.ProsumerId)
                .ToListAsync();
            List<object> lista = new List<object>();
            foreach (var user in allUsers)
            {
                //var cons = GetConsumption(user.Id);
                lista.Add((object)new
                {
                    Id = user.Id,
                    Name = user.Name,
                    Latitude = user.Latitude,
                    Longitude = user.Longitude//,
                    //Consumption = cons
                });
            }
            return lista;
        }
        private double GetConsumption(long userId) {

            if (!context.Devices.Any(u => u.UserId == userId))
                return 0;
            /*
            if (!_sqliteDb.Devices.Any(u => u.DeviceCategoryId == deviceCategoryId))
                return NotFound(new { message = "User with the ID " + userId.ToString() + " does not have registered devices with device category ID " + deviceCategoryId.ToString() + "." });
            */

            if (!context.Devices.Include(d => d.DeviceModel).ThenInclude(dm => dm.DeviceType).ThenInclude(dt => dt.DeviceCategory).Any(d => d.UserId == userId && d.DeviceModel.DeviceType.DeviceCategory.Id == 2))
            {
                return 0;
            }

            return historyService.GetUserEnergyConsumptionForPastMonth(userId, 2);
        }

        public Object CreatePendingUser(PendingUserModel pendingUser)
        {
            var user = context.Users.Where(src => src.Email == pendingUser.Email).FirstOrDefault();
            if (user != null)
                return new HttpRequestException("User with that email address already exists.");
            
            var pending = context.PendingUsers.Where(src => src.Email == pendingUser.Email).FirstOrDefault();
            if (pending != null && pending.ExpireAt > DateTime.Now)
                return new HttpRequestException("Request with that email already exists. Please check your email address.");
            else if(pending != null && pending.ExpireAt > DateTime.Now)
                context.PendingUsers.Remove(pending);
            
            var response = context.PendingUsers.Add(pendingUser);
            context.SaveChanges();
            if (response == null)
                return null;
            return response;
        }

        public Object ConfirmEmailAddress(string key)
        {
            PendingUserModel pendingUser = context.PendingUsers.FirstOrDefault(src => src.ConfirmKey == key);
            if (pendingUser != null)
            {
                //Zahtev postoji
                if (pendingUser.ExpireAt > DateTime.Now)
                {
                    //Jos uvek je validan

                    UserModel user = new UserModel
                    {
                        Username = pendingUser.Username,
                        Name = pendingUser.Name,
                        Password = pendingUser.Password,
                        Blocked = pendingUser.Blocked,
                        RoleId = pendingUser.RoleId,
                        Email = pendingUser.Email,
                        Address = pendingUser.Address,
                        Latitude = pendingUser.Latitude,
                        Longitude = pendingUser.Longitude,
                        SettlementId = pendingUser.SettlementId
                    };

                    var response = context.Users.Add(user);
                    context.Remove(pendingUser);
                    context.SaveChanges();

                    if(response == null)
                    {
                        return new HttpRequestException("Ooops... Something went wrong. Please try again.");
                    }

                    return response;

                }
                else
                {
                    return new HttpRequestException("The confirmation link has expired");
                }
            }
            /*else
            {
                UserModel user = context.Users.FirstOrDefault(src => src.Email == email);
                if (user != null)
                {
                    return new HttpRequestException("This email has already been confirmed.");
                }
            }*/
            return false;
        }

        public object CreateChangeEmailRequest(ChangeEmailModel changeEmail)
        {
            var user = context.Users.Where(src => src.Email == changeEmail.NewEmail).FirstOrDefault();
            if (user != null)
                return new HttpRequestException("User with that email address already exists.");

            ChangeEmailModel changeEmailRequest = context.ChangeEmailModels.Where(src => src.OldEmail == changeEmail.OldEmail).FirstOrDefault();
            if (changeEmailRequest != null && changeEmailRequest.ExpireAt > DateTime.Now)
                return new HttpRequestException("You've already created request to change email address. Check your email inbox.");
            else if (changeEmailRequest != null && changeEmailRequest.ExpireAt > DateTime.Now)
                context.ChangeEmailModels.Remove(changeEmailRequest);

            var response = context.ChangeEmailModels.Add(changeEmail);
            context.SaveChanges();
            if (response == null)
                return null;
            return response;
        }

        public object ConfirmChageOfEmailAddress(string key)
        {
            var changeEmail = context.ChangeEmailModels.Where(src => src.ChangeEmailKey.Equals(key)).FirstOrDefault();
            if(changeEmail == null)
            {
                return new HttpRequestException("There is no request with such a key.");
            }
            else if(changeEmail != null && changeEmail.ExpireAt < DateTime.Now)
            {
                context.ChangeEmailModels.Remove(changeEmail);
                return new HttpRequestException("Confirmation link has been expired. Please create new request.");
            }
            ChangeEmailModel model = (ChangeEmailModel)changeEmail;
            UserModel user = context.Users.Find(model.UserId);
            user.Email = model.NewEmail;

            context.Users.Update(user);
            context.ChangeEmailModels.Remove(model);

            context.SaveChanges();
            return new OkResult();
        }
    }
}
