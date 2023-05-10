using Server.DTOs;
using Server.DTOs.Responses;
using Server.Filters;
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
        Task<DataPage<UserDetailsDTO>> GetPageOfUsers(int pageNumber, int itemsPerPage, long roleId, long myId, UserFilterModel userFilterModel);
        Task<DataPage<ProsumerForDSOResponseDTO>> GetPageOfUsersForDSO(int pageNumber, int itemsPerPage, long cityId, long loggedCityId, UserFilterModel userFilterModel, ProsumerDSOFilterModel prosumerDSOFilter);
        Task<List<RoleModel>> GetAllRoles();

        Task<List<object>> GetAllProsumers(string zone,int city);
        object CreatePendingUser(PendingUserModel pendingUser);
        object ConfirmEmailAddress(string key);
        object CreateChangeEmailRequest(ChangeEmailModel changeEmail);
        object ConfirmChageOfEmailAddress(string key);

    }
}
