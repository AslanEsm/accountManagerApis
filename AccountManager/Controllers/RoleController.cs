using common.Utilities.Paging;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Threading.Tasks;
using ViewModels.Role;
using WebFramework.Filters;

namespace AccountManager.Controllers
{
    [ApiResultFilter]
    public class RoleController : SiteBaseController
    {
        #region Constructor

        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        #endregion Constructor

        //api/role/Get
        [HttpGet("Get")]
        public async Task<IActionResult> Get([FromQuery] FilterRole filterRole)
        {
            var roles = await _roleService
                .FilterRolesAsync(filterRole);

            Response.AddPagination(
                roles.CurrentPage,
                roles.PageSize,
                roles.TotalCount,
                roles.TotalPage);

            return Ok(roles);
        }

        //api/role/Add
        [HttpPost("Add")]
        public async Task<IActionResult> Add(CreateRole roleModel)
        {
            var res = await _roleService.CreateAsync(roleModel);

            return Ok(res);
        }
    }
}