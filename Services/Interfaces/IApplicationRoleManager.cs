using System.Collections.Generic;
using Entities.Role;
using ViewModels.Role;


namespace Services.Interfaces
{
    public interface IApplicationRoleManager
    {
        List<Role> GetAllRoles();
        List<RoleViewModel> GetAllRolesAndUsersCount();
    }
}