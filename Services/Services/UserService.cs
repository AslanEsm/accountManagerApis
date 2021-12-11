using AngularEshop.Data.Contracts;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using common.Utilities;
using common.Utilities.Paging;
using Common.Exceptions;
using Common.Utilities;
using Entities;
using Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ViewModels.User;

namespace Services.Services
{
    public class UserService : IUserService
    {
        #region Constructor

        private readonly IRepository<User> _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IRoleService _roleService;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IMapper _mapper;
        private IConfigurationProvider _configuration;

        public UserService(
            IRepository<User> userRepository,
            UserManager<User> userManager,
            IRoleService roleService,
            IRepository<UserRole> userRoleRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleService = roleService;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
        }

        #endregion Constructor

        #region Methods

        public async Task<bool> IsUserExistByEmail(string email)
        {
            var isExist = await _userRepository.TableNoTracking
                .AnyAsync(u => u.Email == email);

            return isExist;
        }

        public async Task<bool> IsUserExistByUserName(string userName)
        {
            var isExist = await _userRepository.TableNoTracking
                .AnyAsync(u => u.UserName == userName);

            return isExist;
        }

        #endregion Methods

        #region UserRole

        public async Task<IList<string>> GetAllUserRolesAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new NotFoundException();

            var user = await _userRepository.TableNoTracking
                .Include(u => u.UserRoles)
                .SingleOrDefaultAsync(c => c.Id == Convert.ToInt64(userId));

            if (user == null)
                throw new NotFoundException("همچین کاربری وجود ندارد.");

            var userRoles = await _userManager.GetRolesAsync(user);

            return userRoles;
        }

        public async Task<bool> AddRolesToUserAsync(long userId, string[] roles)
        {
            var user = await GetUserAsync(userId.ToString());

            var userRoles = await GetAllUserRolesAsync(userId.ToString());

            var validRoles = roles.Where(r => !userRoles.Contains(r))
                .ToList();

            var result = await _userManager.AddToRolesAsync(user, validRoles);

            if (!result.Succeeded)
                throw new AppException("خطایی اتفاق افتاد لطفا دوباره تلاش کنید");

            return result.Succeeded;
        }

        public async Task<bool> AddRoleToUser(long userId, string role)
        {
            var user = await GetUserAsync(userId.ToString());

            var result = await _userManager.IsInRoleAsync(user, role);

            if (result)
                throw new BadRequestException("کاربر هم اکنون این نقش را دارد,امکان افزودن این نقش نیست ");

            var res = await _userManager.AddToRoleAsync(user, role);

            if (!res.Succeeded)
                throw new AppException("خطایی اتفاق افتاد,لطفا مجددا تلاش کنید");

            return res.Succeeded;
        }

        #endregion UserRole

        #region User

        public async Task<User> GetUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new NotFoundException();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new NotFoundException("همچین کاربری وجود ندارد.");

            return user;
        }

        public async Task<PagedList<UserViewModel>> FilterUserAsync(FilterUser filterUser)
        {
            var query = _userRepository.TableNoTracking;

            #region oldCode

            // var query = _userRepository.TableNoTracking
            //.Select(u => new UserViewModel()
            //{
            //    Id = u.Id,
            //    FirstName = u.FirstName,
            //    LastName = u.LastName,
            //    Email = u.Email,
            //    PhoneNumber = u.PhoneNumber,
            //    BirthDate = u.BirthDate.ToShamsiDateTime("yyyy - MM - dd hh: mm:ss"),
            //    Image = u.Image,
            //    IsActive = u.IsActive,
            //    CreateDate = u.CreateDate.ToShamsiDateTime("yyyy - MM - dd hh: mm:ss"),
            //    LastUpdate = u.LastUpdate.ToShamsiDateTime("yyyy - MM - dd hh: mm:ss"),
            //    LastVisit = u.LastVisit.NullableToShamsiDateTime("yyyy - MM - dd hh: mm:ss"),
            //    UserName = u.UserName,
            //    Roles = u.UserRoles.Select(d => d.Role.Name)
            //});

            #endregion oldCode

            #region search

            if (filterUser.Search != null)
            {
                query = query.Where(u =>
                    u.FirstName.Contains(filterUser.Search) ||
                    u.LastName.Contains(filterUser.Search) ||
                    u.UserName.Contains(filterUser.Search) ||
                    u.Email.Contains(filterUser.Search));
            }

            if (filterUser.Email != null)
            {
                query = query.Where(u => u.Email.Contains(filterUser.Email));
            }
            if (filterUser.FirstName != null)
            {
                query = query.Where(u => u.FirstName.Contains(filterUser.FirstName));
            }
            if (filterUser.LastName != null)
            {
                query = query.Where(u => u.LastName.Contains(filterUser.LastName));
            }
            if (filterUser.UserName != null)
            {
                query = query.Where(u => u.UserName.Contains(filterUser.UserName));
            }

            #endregion search

            #region Dates

            if (filterUser.FromBirthDate != null || filterUser.ToBirthDate != null)
            {
                query = query.BetweenDates("BirthDate",
                    filterUser.FromBirthDate,
                    filterUser.ToBirthDate);
            }

            if (filterUser.FromRegisterDate != null || filterUser.ToRegisterDate != null)
            {
                query = query.BetweenDates("RegisterDate",
                    filterUser.FromRegisterDate,
                    filterUser.ToRegisterDate);
            }

            if (filterUser.FromUpdateTime != null || filterUser.FromUpdateTime != null)
            {
                query = query.BetweenDates("UpdateDate",
                    filterUser.FromUpdateTime,
                    filterUser.ToUpdateTime);
            }

            if (filterUser.FromLastVisitDate != null || filterUser.FromLastVisitDate != null)
            {
                query = query.BetweenDates("LastVisitDateTime",
                    filterUser.FromLastVisitDate,
                    filterUser.ToLastVisitDate);
            }

            #endregion Dates

            #region Order

            if (filterUser.SortBy != null && !string.IsNullOrEmpty(filterUser.SortBy))
                query = (IQueryable<User>)query.Sort(filterUser.SortBy, filterUser.Reverse);

            #endregion Order

            var x = query.ProjectTo<UserViewModel>(_mapper.ConfigurationProvider);

            var pagedList = await PagedList<UserViewModel>
                .CreatAsync(x, filterUser.PageNumber, filterUser.PageSize);

            return pagedList;
        }

        public async Task<UserViewModel> FindUserWithRoleByIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new NotFoundException();

            var user = await _userRepository.TableNoTracking
                .Select(u => new UserViewModel()
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    BirthDate = u.BirthDate.ToShamsiDateTime("yyyy-MM-dd hh:mm:ss"),
                    Image = u.Image,
                    IsActive = u.IsActive,
                    CreateDate = u.CreateDate.ToShamsiDateTime("yyyy-MM-dd hh:mm:ss"),
                    LastUpdate = u.LastUpdate.ToShamsiDateTime("yyyy-MM-dd hh:mm:ss"),
                    LastVisit = u.LastVisit.NullableToShamsiDateTime("yyyy-MM-dd hh:mm:ss"),
                    UserName = u.UserName,
                    Roles = u.UserRoles.Select(d => d.Role.Name),
                    AccessFailedCount = u.AccessFailedCount,
                    TwoFactorEnabled = u.TwoFactorEnabled,
                    LockoutEnabled = u.LockoutEnabled,
                    PhoneNumberConfirmed = u.PhoneNumberConfirmed,
                    EmailConfirmed = u.EmailConfirmed,
                    LockoutEnd = (u.LockoutEnd == null) ? "" : u.LockoutEnd.Value.DateTime.ToLocalTime().ToShamsiDateTime("yyyy-MM-dd hh:mm:ss")
                }).SingleOrDefaultAsync(u => u.Id == Convert.ToInt64(userId));

            if (user == null)
                throw new NotFoundException("این کاربر وجود ندارد");

            return user;
        }

        #region MyRegion

        //private async Task<List<UserRole>> CreateUserRoleModelList(List<string> roleList, long userId)
        //{
        //    var list = new List<UserRole>();
        //    var roles = await _roleService.GetRolesByName(roleList);
        //    foreach (var role in roles)
        //    {
        //        var userRole = new UserRole()
        //        {
        //            RoleId = role.Id,
        //            UserId = userId
        //        };

        //        list.Add(userRole);
        //    }

        //    return list;
        //}

        #endregion MyRegion

        public async Task<UserViewModel> UpdateUserAsync(EditUser model, CancellationToken cancellationToken)

        {
            IdentityResult result;
            var user = await _userManager.FindByIdAsync(model.Id.ToString());

            #region UpdateRoles

            if (model.Roles != null)
            {
                //update roles
                var recentRoles = await GetAllUserRolesAsync(user.Id.ToString());
                var deleteRoles = recentRoles.Except(model.Roles).ToList();
                var addRoles = model.Roles.Except(recentRoles).ToList();

                #region oldCode

                //if (deleteRoles.IsAny())
                //{
                //    var list = await CreateUserRoleModelList(deleteRoles, model.Id);
                //    await _userRoleRepository.DeleteRangeAsync(list, cancellationToken);

                //}
                //if (addRoles.IsAny())
                //{
                //   var list = await CreateUserRoleModelList(addRoles, model.Id);
                //    await _userRoleRepository.AddRangeAsync(list, cancellationToken);

                //}

                #endregion oldCode

                if (deleteRoles.IsAny())
                {
                    result = await _userManager.RemoveFromRolesAsync(user, deleteRoles);
                    if (!result.Succeeded)
                        throw new AppException("خطای حذف نقش های کاربر ");
                }

                if (addRoles.IsAny())
                {
                    result = await _userManager.AddToRolesAsync(user, addRoles);

                    if (!result.Succeeded)
                        throw new AppException("خطای افزودن نقش های کاربر ");
                }
            }

            #endregion UpdateRoles

            #region UpdateUser

            //update other properties

            var rDto = _mapper.Map<EditUser, User>(model, user);
            var updateResult = await _userManager.UpdateAsync(rDto);
            if (!updateResult.Succeeded)
            {
                List<IdentityError> errorList = updateResult.Errors.ToList();
                var errors = string.Join(", ", errorList.Select(e => e.Description));
                throw new AppException($"{errors}");
            }

            var resultDto = await _userRepository.TableNoTracking
                .ProjectTo<UserViewModel>(_configuration)
                .SingleOrDefaultAsync(p => p.Id == model.Id, cancellationToken);

            #region oldCode

            //var resultDto = await _userRepository.TableNoTracking.Select(u => new UserViewModel()
            //{
            //    Id = u.Id,
            //    FirstName = u.FirstName,
            //    LastName = u.LastName,
            //    Email = u.Email,
            //    PhoneNumber = u.PhoneNumber,
            //    BirthDate = u.BirthDate,
            //    Image = u.Image,
            //    IsActive = u.IsActive,
            //    CreateDate = u.CreateDate,
            //    LastUpdate = u.LastUpdate,
            //    LastVisit = u.LastVisit,
            //    UserName = u.UserName,
            //    LockoutEnabled = u.LockoutEnabled,
            //    PhoneNumberConfirmed = u.PhoneNumberConfirmed,
            //    TwoFactorEnabled = u.TwoFactorEnabled,
            //    AccessFailedCount = u.AccessFailedCount,
            //    LockoutEnd = u.LockoutEnd,
            //    Roles = u.UserRoles.Select(d => d.Role.Name)
            //}).SingleOrDefaultAsync(c => c.Id == model.Id, cancellationToken);

            #endregion oldCode

            #endregion UpdateUser

            return resultDto;
        }

        public async Task<bool> DeleteUserAsync(string id, CancellationToken cancellationToken)
        {
            var user = await GetUserAsync(id);

            var userRoles = await _userRoleRepository.Table
                .Where(u => u.UserId == Convert.ToInt64(id))
                .ToListAsync();

            await _userRoleRepository.DeleteRangeAsync(userRoles, cancellationToken);

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new AppException("خطای حذف کاربر");

            return result.Succeeded;
        }

        public Task UpdateLastLoginDateAsync(User user)
        {
            user.LastVisit = DateTime.Now;
            return _userManager.UpdateAsync(user);
        }

        #endregion User
    }
}