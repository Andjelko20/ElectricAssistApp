using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs;
using Server.Models;
using System.Linq;
using System.Net;

namespace Server.Services.Implementations
{
    public class UserService : IUserService
    {
        public readonly SqliteDbContext context;
        public readonly ILogger<UserService> logger;
        public UserService(SqliteDbContext context, ILogger<UserService> logger)
        {
            this.context = context;
            this.logger = logger;
        }
        public int GetNumberOfPages(int itemsPerPage,Func<UserModel, bool> filter)
        {
            int numberOfItems = context.Users.Count(filter);
            if(numberOfItems%itemsPerPage==0)
                return numberOfItems/itemsPerPage;
            return numberOfItems / itemsPerPage + 1;
        }
        public async Task<DataPage<object>> GetPageOfUsers(int pageNumber,int itemsPerPage,Func<UserModel,bool> filter)
        {
            DataPage<object> page = new();
            page.NumberOfPages = GetNumberOfPages(itemsPerPage,filter);
            if (page.NumberOfPages == 0)
                throw new HttpRequestException("No items",null,HttpStatusCode.NotFound);
            if (page.NumberOfPages < pageNumber || pageNumber<1)
                throw new HttpRequestException("Invalid page number",null,HttpStatusCode.BadRequest);
            logger.LogInformation(page.NumberOfPages.ToString());
            List<object> users=context.Users
                .Include(user=>user.Role)
                .Where(filter)
                .Skip((pageNumber-1)*itemsPerPage)
                .Take(itemsPerPage)
                .Select(user=>(object)(new {
                    Id = user.Id,
                    Name = user.Name,
                    Username = user.Username,
                    Role = user.Role.Name,
                    Blocked = user.Blocked,
                    Email = user.Email
                }))
                .ToList();
            page.Data = users;
            page.PreviousPage = (pageNumber == 1) ? null : pageNumber - 1;
            page.NextPage = (pageNumber == page.NumberOfPages) ? null : pageNumber + 1;
            return page;
        }

        public Task<UserModel?> GetUserByEmail(string email)
        {
            return context.Users.Include(user=>user.Role).FirstOrDefaultAsync(user => user.Email == email);
        }
        public async Task<UserModel?> GetUserByUsername(string username)
        {
            try
            {
                var user = await context.Users.Include(user => user.Role).FirstOrDefaultAsync(user => user.Username == username);
                return user;
            }
            catch(NullReferenceException ex)
            {
                return null;
            }
        }
        public Task<UserModel?> GetUserById(int id)
        {
            return context.Users.Include(user=>user.Role).FirstOrDefaultAsync(user => user.Id == id);
        }

        public Task<List<RoleModel>> GetAllRoles()
        {
            return context.Roles.ToListAsync();
        }
    }
}
