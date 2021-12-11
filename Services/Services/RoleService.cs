using AngularEshop.Data.Contracts;
using common.Utilities.Paging;
using Common.Exceptions;
using Entities.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Services.Interfaces;
using ViewModels.Role;

namespace Services.Services
{
    public class RoleService : IRoleService
    {
        #region Constructor

        private readonly IRepository<Role> _roleRepository;
        private readonly RoleManager<Role> _roleManager;
        private readonly IApplicationRoleManager _applicationRoleManager;

        public RoleService(IRepository<Role> roleRepository, RoleManager<Role> roleManager,
            IApplicationRoleManager applicationRoleManager)
        {
            _roleRepository = roleRepository;
            _roleManager = roleManager;
            _applicationRoleManager = applicationRoleManager;
        }

        #endregion Constructor

        public async Task<List<Role>> GetAllAsync()
        {
            var roles = await _roleManager.Roles
                .ToListAsync();

            return roles;
        }

        public async Task<PagedList<RoleViewModel>> FilterRolesAsync(FilterRole filterRole)
        {
            var query = _roleRepository.TableNoTracking
                .Select(r => new RoleViewModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    UsersCount = r.UserRoles.Count,
                });

            if (filterRole.Name != null)
            {
                switch (filterRole.SortOrder)
                {
                    case SortOrderResult.Name:
                        query = query.OrderBy(p => p.Name);
                        break;

                    case SortOrderResult.Name_Desc:
                        query = query.OrderByDescending(p => p.Name);
                        break;
                }
            }

            if (filterRole.Name != null &&
                !string.IsNullOrEmpty(filterRole.Name) &&
                !string.IsNullOrWhiteSpace(filterRole.Name))
            {
                query = query.Where(r => r.Name.Contains(filterRole.Name));
            }

            var pagedList = await PagedList<RoleViewModel>
                .CreatAsync(query, filterRole.PageNumber, filterRole.PageSize);

            return pagedList;
        }

        public async Task<Role> CreateAsync(CreateRole model)
        {
            if (model == null)
                throw new BadRequestException();

            var role = new Role()
            {
                Name = model.Name,
                Description = model.Description
            };

            if (await _roleManager.RoleExistsAsync(model.Name))
                throw new BadRequestException("رول وارد شده تکراری است");

            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
                throw new AppException();

            return role;
        }

        public async Task<Role> UpdateAsync(EditRole model)
        {
            if (model == null)
                throw new BadRequestException();

            var role = await _roleManager.FindByIdAsync(model.Id.ToString());

            role.Name = model.Name;
            role.Description = model.Description;

            if (await _roleManager.RoleExistsAsync(model.Name) && (model.RecentRoleName != model.Name))
                throw new BadRequestException("رول وارد شده تکراری است");

            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
                throw new AppException();

            return role;
        }

        public async Task<Role> DeleteAsync(long id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());

            if (role == null)
                throw new NotFoundException();

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
                throw new AppException();

            return role;
        }

        public async Task<List<string>> GetRolesName()
        {
            var roleNames = await _roleManager.Roles
                .Select(r => r.Name)
                .ToListAsync();

            return roleNames;
        }

        public async Task<Role> GetRoleByName(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            return role;
        }

        public async Task<List<Role>> GetRolesByName(List<string> rolesName)
        {
            var roleList = new List<Role>();
            foreach (var roleName in rolesName)
            {
                var role = await GetRoleByName(roleName);
                roleList.Add(role);
            }

            return roleList;
        }

    }
}