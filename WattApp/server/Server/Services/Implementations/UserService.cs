using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Polly;
using Server.Data;
using Server.DTOs;
using Server.DTOs.Responses;
using Server.Filters;
using Server.Models;
using Server.Models.DropDowns.Devices;
using Server.Models.DropDowns.Location;
using Server.Utilities;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<DataPage<UserDetailsDTO>> GetPageOfUsers(int pageNumber, int itemsPerPage, long roleId, long myId, UserFilterModel userFilterModel)
        {
            DataPage<UserDetailsDTO> page = new();
            IQueryable<UserModel> users = null;

            if(roleId == Roles.OperaterId)
            {
                users = (IQueryable<UserModel>)context.Users
                .Include(user => user.Role)
                .Include(user => user.Settlement)
                .Include(user => user.Settlement.City)
                .Include(user => user.Settlement.City.Country)
                .Where((user) => user.RoleId == Roles.ProsumerId && user.RoleId != Roles.SuperadminId && user.Id != myId);
            }
            else if(roleId == Roles.AdminId)
            {
                users = (IQueryable<UserModel>)context.Users
                .Include(user => user.Role)
                .Include(user => user.Settlement)
                .Include(user => user.Settlement.City)
                .Include(user => user.Settlement.City.Country)
                .Where((user) => user.RoleId != Roles.SuperadminId && user.Id != myId);
            }
            else
            {
                users = (IQueryable<UserModel>)context.Users
                .Include(user => user.Role)
                .Include(user => user.Settlement)
                .Include(user => user.Settlement.City)
                .Include(user => user.Settlement.City.Country)
                .Where((user) => user.Id != myId);
            }
            

            if(users == null)
                throw new HttpRequestException("No items found in database.", null, HttpStatusCode.NotFound);

            users = UserFilter.applyFilters(users, userFilterModel);

            if (!users.Any()) throw new HttpRequestException("There is no devices!", null, System.Net.HttpStatusCode.NotFound);

            int maxPageNumber;
            if (users.Count() % itemsPerPage == 0) maxPageNumber = users.Count() / itemsPerPage;
            else maxPageNumber = users.Count() / itemsPerPage + 1;

            if (pageNumber < 1 || pageNumber > maxPageNumber) throw new HttpRequestException("Invalid page number!", null, System.Net.HttpStatusCode.BadRequest);
            if (itemsPerPage < 1) throw new HttpRequestException("Invalid page size number!", null, System.Net.HttpStatusCode.BadRequest);

            users = users.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage);

            List<UserModel> userModels = users.ToList();
            List<UserDetailsDTO> userDetailsDTOs = new List<UserDetailsDTO>();
            foreach(UserModel user in userModels)
            {
                UserDetailsDTO detailsDTO = new UserDetailsDTO(user);
                userDetailsDTOs.Add(detailsDTO);
            }
            
            page.Data = userDetailsDTOs;
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

        public async Task<List<object>> GetAllProsumers(string zone,int city)
        {
            List<UserModel> allUsers = await context
                .Users
                .Include(user=>user.Settlement)
                .Where(user => user.RoleId == Roles.ProsumerId)
                .ToListAsync();
            List<object> lista = new List<object>();
            foreach (var user in allUsers)
            {
                var cons = GetConsumption(user.Id);
                if (zone == "1" && cons > 350)
                    continue;
                if (zone == "2" && ( cons <= 350 || cons > 1600 ) )
                    continue;
                if (zone == "3" && cons <= 1600)
                    continue;
                if (city != 0 && user.Settlement.CityId != city)
                    continue;
                lista.Add((object)new
                {
                    Id = user.Id,
                    Name = user.Name,
                    Latitude = user.Latitude,
                    Longitude = user.Longitude,
                    Consumption = cons,
                    CityId=user.Settlement.CityId,
                    Address=user.Address
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

        public object CreatePendingUser(PendingUserModel pendingUser)
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

        public object ConfirmEmailAddress(string key)
        {
            PendingUserModel pendingUser = context.PendingUsers.FirstOrDefault(src => src.ConfirmKey == key);
            if (pendingUser == null)
            {
                return new HttpRequestException("There is no pending request with such a key");
            }
            else
            {
                if (pendingUser.ExpireAt < DateTime.Now)
                {
                    context.PendingUsers.Remove(pendingUser);
                    context.SaveChanges();
                    return new HttpRequestException("Confirmation link has been expired");
                }
                else
                {
                    UserModel userModel = context.Users.FirstOrDefault(src => src.Username == pendingUser.Username || src.Email == pendingUser.Email);
                    if (userModel != null)
                    {
                        return new HttpRequestException("Ooops... Looks like there is a user with such username or email.");
                    }
                    else
                    {
                        userModel = new UserModel()
                        {
                            Username = pendingUser.Username,
                            Password = pendingUser.Password,
                            Name = pendingUser.Name,
                            Address = pendingUser.Address,
                            Longitude = pendingUser.Longitude,
                            Latitude = pendingUser.Latitude,
                            Email = pendingUser.Email,
                            RoleId = pendingUser.RoleId,
                            Blocked = pendingUser.Blocked,
                            SettlementId = pendingUser.SettlementId
                        };
                        UserModel response = context.Users.Add(userModel).Entity;
                        context.PendingUsers.Remove(pendingUser);
                        if (response == null)
                        {
                            return new HttpRequestException("Ooops... Something went wrong! Please try again");
                        }
                        context.SaveChanges();
                        return response;
                    }
                }
            }
        }

        public object CreateChangeEmailRequest(ChangeEmailModel changeEmail)
        {
            UserModel user = null;
            user = context.Users.Where(src => src.Email == changeEmail.NewEmail).FirstOrDefault();
            if(user != null)
            {
                return new HttpRequestException("User with that email address already exists.");
            }

            ChangeEmailModel changeEmailModel = null;
            changeEmailModel = context.ChangeEmailModels.Where(src => src.OldEmail == changeEmail.OldEmail).FirstOrDefault();
            if(changeEmailModel != null)
            {
                if(changeEmailModel.ExpireAt > DateTime.Now)
                {
                    return new HttpRequestException("You've already created request to change email address. Check your email inbox.");
                }
                else
                {
                    context.ChangeEmailModels.Remove(changeEmailModel);
                }
            }

            ChangeEmailModel model = context.ChangeEmailModels.Add(changeEmail).Entity;
            context.SaveChanges();
            return model;
        }

        public object ConfirmChageOfEmailAddress(string key)
        {
            ChangeEmailModel changeEmailModel = context.ChangeEmailModels.FirstOrDefault(src => src.ChangeEmailKey == key);

            if(changeEmailModel == null)
            {
                return new HttpRequestException("Sorry! But there is no request with that key.");
            }
            else
            {
                if(changeEmailModel.ExpireAt < DateTime.Now)
                {
                    context.ChangeEmailModels.Remove(changeEmailModel);
                    context.SaveChanges();
                    return new HttpRequestException("Sorry! But link has been expired");
                }

                UserModel user = null;
                user = context.Users.Where(src => src.Email == changeEmailModel.NewEmail).FirstOrDefault();
                if(user != null)
                {
                    context.ChangeEmailModels.Remove(changeEmailModel);
                    context.SaveChanges();
                    return new HttpRequestException("Someone is already using that email address.");
                }

                user = context.Users.Where(src => src.Email == changeEmailModel.OldEmail).FirstOrDefault();
                user.Email = changeEmailModel.NewEmail;
                UserModel response = context.Users.Update(user).Entity;
                context.ChangeEmailModels.Remove(changeEmailModel);
                context.SaveChanges();

                return response;
            }
        }
    }
}
