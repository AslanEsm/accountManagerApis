using common.Utilities.Paging;
using Entities.Role;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModels.Role;

namespace Services.Interfaces
{
    public interface IRoleService
    {
        Task<List<Role>> GetAllAsync();

        Task<PagedList<RoleViewModel>> FilterRolesAsync(FilterRole filterRole);

        Task<Role> CreateAsync(CreateRole model);

        Task<Role> UpdateAsync(EditRole model);

        Task<Role> DeleteAsync(long id);

        Task<List<string>> GetRolesName();

        Task<Role> GetRoleByName(string roleName);

        Task<List<Role>> GetRolesByName(List<string> rolesName);
    }
}