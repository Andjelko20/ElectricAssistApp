using Server.DTOs;
using Server.Models;

namespace Server.Services
{
    public interface IUserService
    {
        Task<UserModel?>? GetUserById(int id);
        Task<UserModel?>? GetUserByEmail(string email);
        Task<UserModel?> GetUserByUsername(string username);
        Task<DataPage<object>> GetPageOfUsers(int pageNumber, int itemsPerPage, Func<UserModel, bool> filter);
        Task<List<RoleModel>> GetAllRoles();

    }
}
