using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using common.Utilities.Paging;
using Entities;
using Entities.User;
using ViewModels;
using ViewModels.User;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> IsUserExistByEmail(string email);
        Task<bool> IsUserExistByUserName(string userName);
        Task<IList<string>> GetAllUserRolesAsync(string userId);
        Task<bool> AddRolesToUserAsync(long userId, string[] roles);
        Task<bool> AddRoleToUser(long userId, string role);
        Task<User> GetUserAsync(string userId);
        Task<PagedList<UserViewModel>> FilterUserAsync(FilterUser filterUser);
        Task<UserViewModel> FindUserWithRoleByIdAsync(string userId);
        Task<UserViewModel> UpdateUserAsync(EditUser model, CancellationToken cancellationToken);
        Task<bool> DeleteUserAsync(string id, CancellationToken cancellationToken);
        Task UpdateLastLoginDateAsync(User user);

    }
}