using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Services.Interfaces;
using ViewModels.Role;

namespace Entities.Role
{
    public class ApplicationRoleManager : RoleManager<Role>, IApplicationRoleManager
    {
        #region Constructor

        private readonly IdentityErrorDescriber _errors;
        private readonly ILookupNormalizer _keyNormalizer;
        private readonly ILogger<ApplicationRoleManager> _logger;
        private readonly IEnumerable<IRoleValidator<Role>> _roleValidators;
        private readonly IRoleStore<Role> _store;

        public ApplicationRoleManager(
            IRoleStore<Role> store,
            IEnumerable<IRoleValidator<Role>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager<Role>> logger,
            ILogger<ApplicationRoleManager> logger2
        ) : base(store, roleValidators, keyNormalizer, errors, logger)
        {
            _errors = errors;
            _keyNormalizer = keyNormalizer;
            _logger = logger2;
            _roleValidators = roleValidators;
            _store = store;
        }

        #endregion Constructor

        public List<Role> GetAllRoles()
        {
            var roles = Roles.ToList();
            return roles;
        }

        public List<RoleViewModel> GetAllRolesAndUsersCount()
        {
            return Roles.Select(role => new RoleViewModel()
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                UsersCount = role.UserRoles.Count
            }).ToList();
        }
    }
}