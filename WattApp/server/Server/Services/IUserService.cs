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

        Object CreatePendingUser(PendingUserModel pendingUser);
        Object ConfirmEmailAddress(string email);
        Object CreateChangeEmailRequest(ChangeEmailModel changeEmail);
        Object ConfirmChageOfEmailAddress(string key);

    }
}
