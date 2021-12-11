using common.Utilities.Paging;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ViewModels.User;
using WebFramework.Filters;

namespace AccountManager.Controllers
{
    [ApiResultFilter]
    //[Authorize]
    public class UserController : SiteBaseController
    {
        #region Constructor

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion Constructor
        //api/user/get
        [HttpGet("[action]")]
        public async Task<IActionResult> Get([FromBody] FilterUser filterUser)
        {
            var users = await _userService
                .FilterUserAsync(filterUser);

            Response.AddPagination(
                users.CurrentPage,
                users.PageSize,
                users.TotalCount,
                users.TotalPage);

            return Ok(users);
        }
        //api/user/get/id
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await _userService.FindUserWithRoleByIdAsync(id);

            return Ok(user);
        }
        //api/user/update
        [HttpPost("[action]")]
        public async Task<IActionResult> Update(EditUser model, CancellationToken cancellationToken)
        {
            var user = await _userService.UpdateUserAsync(model, cancellationToken);

            return Ok(user);
        }
        //api/user/delete/id
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
        {
            var result = await _userService.DeleteUserAsync(id.ToString(), cancellationToken);
            return Ok();
        }
    }
}