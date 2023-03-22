using Server.Models;

namespace Server.Services
{
    public interface IUserService
    {
        Task<UserModel?> GetUserById(int id);
        Task<UserModel?> GetUserByEmail(string email);
        Task<UserModel?> GetUserByUsername(string email);
        Task<UserModel?> GetPageOfUsers(int page);
        //Task<bool> CheckPassword(string password);

    }
}
