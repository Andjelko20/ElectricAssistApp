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
using System.Net;

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
            if (pending != null && pending.ExpireAt < DateTime.Now)
                return new HttpRequestException("Request with that email already exists. Please check your email address.");
            else if(pending != null && pending.ExpireAt > DateTime.Now)
                context.PendingUsers.Remove(pending);
            
            var response = context.PendingUsers.Add(pendingUser);
            context.SaveChanges();
            if (response == null)
                return null;
            return response;
        }


        /*PendingUserModel GetPendingUserByEmail(string email)
        {
            return context.PendingUsers.FirstOrDefault(src => src.Email == email);
        }

        PendingUserModel DeletePendingUser(PendingUserModel pendingUser)
        {
            var user = context.PendingUsers.Remove(pendingUser);
            if (user == null)
                return null;
            return pendingUser;
        }

        List<PendingUserModel> DeleteAllExpiredPendingUsers()
        {
            List<PendingUserModel> pendingUserModels = context.PendingUsers.Where(src => src.ExpireAt > DateTime.Now).ToList();
            foreach (PendingUserModel pendingUser in pendingUserModels)
            {
                context.PendingUsers.Remove(pendingUser);
            }
            return pendingUserModels;
        }*/
    }
}
