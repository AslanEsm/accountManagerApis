using Common.Exceptions;
using Common.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using ViewModels.Account;
using ViewModels.Results;
using WebFramework.Filters;

namespace AccountManager.Controllers
{
    [ApiResultFilter]
    public class AccountController : SiteBaseController
    {
        private readonly IAccountService _accountService;
        private readonly IJwtService _jwtService;

        public AccountController(IAccountService accountService, IJwtService jwtService)
        {
            _accountService = accountService;
            _jwtService = jwtService;
        }

        #region SignUp

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp(RegisterDto model)
        {
            var res = await _accountService.Register(model);

            switch (res.Result)
            {
                case Result.DuplicateEmail:
                    return BadRequest(Result.DuplicateEmail.ToDisplay());

                case Result.DuplicateUserName:
                    return BadRequest(Result.DuplicateUserName.ToDisplay());

                case Result.ServerError:
                    throw new LogicException(Result.ServerError.ToDisplay());

                case Result.AddToRoleOperation:
                    throw new LogicException(Result.AddToRoleOperation.ToDisplay());

                case Result.Success:
                    return Ok(Result.Success.ToDisplay());
            }

            throw new AppException("خطایی اتفاق افتاد , لطفا مجددا تلاش کنید.");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SignUpByAdmin(RegisterByAdminDto model)
        {
            var res = await _accountService.RegisterByAdmin(model);

            switch (res.Result)
            {
                case Result.DuplicateEmail:
                    return BadRequest(Result.DuplicateEmail.ToDisplay());

                case Result.DuplicateUserName:
                    return BadRequest(Result.DuplicateUserName.ToDisplay());

                case Result.ServerError:
                    throw new LogicException(Result.ServerError.ToDisplay());

                case Result.AddToRoleOperation:
                    throw new LogicException(Result.AddToRoleOperation.ToDisplay());

                case Result.Success:
                    return Ok(Result.Success.ToDisplay());
            }

            throw new AppException("خطایی اتفاق افتاد , لطفا مجددا تلاش کنید.");
        }

        #endregion SignUp

        #region SignIn

        //api/account/SignIn
        [HttpPost("[action]")]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            var res = await _accountService.SignIn(model);
            return Ok(res);
        }

        #endregion SignIn

        //api/account/ActivateAccount
        [HttpPost("[action]")]
        public async Task<IActionResult> ActivateAccount(long userId, string code)
        {
            await _accountService.ConfirmEmail(userId.ToString(), code);
            return Ok();
        }

        //api/account/forgetPassword
        [HttpPost("[action]")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            await _accountService.ForgetPassword(model);
            return Ok();
        }

        //api/account/ResetPassword
        [HttpPost("[action]")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            await _accountService.ResetPassword(model);
            return Ok();
        }

        //api/account/ChangeLockOutStatus
        [HttpPost("[action]")]
        public async Task<IActionResult> ChangeLockOutStatus(ChangeLockOutStatus model)
        {
            var res = await _accountService.ChangeLockOutEnable(model);
            return Ok(res);
        }

        //api/account/LockUserAccount/id
        [HttpPost("LockUserAccount/{id}")]
        public async Task<IActionResult> LockUserAccountLockUserAccount(string id)
        {
            var res = await _accountService.LockUserAccount(id);
            return Ok(res);
        }

        //api/account/UnLockUserAccount/id
        [HttpPost("UnLockUserAccount/{id}")]
        public async Task<IActionResult> UnLockUserAccount(string id)
        {
            var res = await _accountService.UnLockUserAccount(id);
            return Ok(res);
        }

        //api/account/ChangeActiveStatus
        [HttpPost("[action]")]
        public async Task<IActionResult> ChangeActiveStatus(ChangeActiveStatus model, CancellationToken cancellationToken)
        {
            var res = await _accountService.ChangeActiveStatus(model, cancellationToken);
            return Ok(res);
        }

        //api/account/ResetPasswordByAdmin 
        [HttpPost("[action]")]
        public async Task<IActionResult> ResetPasswordByAdmin(ResetPasswordByAdmin model)
        {
            var res = await _accountService.ResetPasswordByAdmin(model);
            return Ok(res);
        }

        //api/account/SendSms 
        [HttpGet("[action]")]
        public async Task<IActionResult> SendSms()
        {
            var res = await _accountService.SendSms();
            return Ok(res);
        }
        //api/account/ChangeTwoFactorEnable/id
        [HttpPost("ChangeTwoFactorEnabled/{id}")]
        public async Task<IActionResult> ChangeTwoFactorEnabled(string id, CancellationToken cancellationToken)
        {
            var res = await _accountService.ChangeTwoFactorStatus(id, cancellationToken);
            return Ok(res);
        }

        //api/account/ChangeEmailConfirmed/id
        [HttpPost("ChangeEmailConfirmed/{id}")]
        public async Task<IActionResult> ChangeEmailConfirmed(string id, CancellationToken cancellationToken)
        {
            var res = await _accountService.ChangeEmailConfirmedStatus(id, cancellationToken);
            return Ok(res);
        }
        //api/account/ChangePhoneNumberConfirmed/id
        [HttpPost("ChangePhoneNumberConfirmed/{id}")]
        public async Task<IActionResult> ChangePhoneNumberConfirmed(string id, CancellationToken cancellationToken)
        {
            var res = await _accountService.ChangePhoneNumberConfirmedStatus(id, cancellationToken);
            return Ok(res);
        }

        //api/account/SendVerifyCode
        [HttpPost("[action]")]
        public async Task<IActionResult> SendVerifyCode(SendVerifyCodeDto sendVerifyCodeDto)
        {
            await _accountService.SendVerifyCode(sendVerifyCodeDto);
            return Ok();
        }
        //api/account/VerifyCode
        [HttpPost("[action]")]
        public async Task<IActionResult> VerifyCode(VerifyCodeDto model)
        {
            var res = await _accountService.VerifyCode(model);
            return Ok(res);
        }

        //api/account/ChangePassword
        [HttpPost("[action]")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            var userId = User.Identity.GetUserId();
            var res = await _accountService.ChangePasswordAsync(userId, model);
            return Ok(res);
        }
    }
}