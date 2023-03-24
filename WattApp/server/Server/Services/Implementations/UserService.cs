using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.DTOs;
using Server.Models;
using System.Linq;

namespace Server.Services.Implementations
{
    public class UserService : IUserService
    {
        SqliteDbContext context;
        public UserService(SqliteDbContext context)
        {
            this.context = context;
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
                throw new Exception();
            if (page.NumberOfPages < pageNumber || pageNumber<1)
                throw new Exception();
            List<object> users = context.Users
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

        public async Task<UserModel?> GetUserByEmail(string email)
        {
            return await context.Users.Include(user=>user.Role).FirstOrDefaultAsync(user => user.Email == email);
        }
        public async Task<UserModel> GetUserByUsername(string username)
        {
            return await context.Users.Include(user => user.Role).FirstOrDefaultAsync(user => user.Username== username);
        }
        public async Task<UserModel?> GetUserById(int id)
        {
            return await context.Users.Include(user=>user.Role).FirstOrDefaultAsync(user => user.Id == id);
        }
    }
}
