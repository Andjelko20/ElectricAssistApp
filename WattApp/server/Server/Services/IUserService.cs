using Server.DTOs;
using Server.DTOs.Responses;
using Server.Models;

namespace Server.Services
{
    public interface IUserService
    {
        Task<UserModel?>? GetUserById(long id);
        Task<UserModel?>? Login(string username);
        Task<UserModel?>? GetUserByEmail(string email);
        Task<UserModel?> GetUserByUsername(string username);
        Task<DataPage<UserDetailsDTO>> GetPageOfUsers(int pageNumber, int itemsPerPage, Func<UserModel, bool> filter);
        Task<List<RoleModel>> GetAllRoles();
        Task<List<object>> GetAllProsumers();

        /*PendingUserModel CreatePendingUser(PendingUserModel pendingUser);
        PendingUserModel GetPendingUserByEmail(string email);
        PendingUserModel DeletePendingUser(PendingUserModel pendingUser);
        List<PendingUserModel> DeleteAllExpiredPendingUsers();*/
        Object CreatePendingUser(PendingUserModel pendingUser);

    }
}
