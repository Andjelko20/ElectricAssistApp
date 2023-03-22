using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;

namespace Server.Services.Implementations
{
    public class UserService : IUserService
    {
        SqliteDbContext context;
        public UserService(SqliteDbContext context)
        {
            this.context = context;
        }

        public Task<UserModel?> GetPageOfUsers(int page)
        {
            throw new NotImplementedException();
        }

        public async Task<UserModel?> GetUserByEmail(string email)
        {
            return await context.Users.Include(user=>user.Role).FirstOrDefaultAsync(user => user.Email == email);
        }
        public async Task<UserModel?> GetUserByUsername(string username)
        {
            return await context.Users.Include(user => user.Role).FirstOrDefaultAsync(user => user.Email == username);
        }
        public async Task<UserModel?> GetUserById(int id)
        {
            return await context.Users.Include(user=>user.Role).FirstOrDefaultAsync(user => user.Id == id);
        }
    }
}
