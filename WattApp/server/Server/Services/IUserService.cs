using Server.DTOs;
using Server.Models;

namespace Server.Services
{
    public interface IUserService
    {
        Task<UserModel?> GetUserById(int id);
        Task<UserModel?> GetUserByEmail(string email);
        Task<UserModel> GetUserByUsername(string email);
        Task<DataPage<object>> GetPageOfUsers(int pageNumber, int itemsPerPage, Func<UserModel, bool> filter);
        //Task<bool> CheckPassword(string password);

    }
}
