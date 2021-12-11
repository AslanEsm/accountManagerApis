using System.Threading;
using System.Threading.Tasks;
using ViewModels.Account;
using ViewModels.Results;
using ViewModels.User;

namespace Services.Interfaces
{
    public interface IAccountService
    {
        Task<RegisterResult> Register(RegisterDto registerModel);

        Task<RegisterResult> RegisterByAdmin(RegisterByAdminDto registerModel);

        Task<bool> ConfirmEmail(string userId, string code);

        Task<SignInResponseDto> SignIn(SignInViewModel signInModel);

        Task SendVerifyCode(SendVerifyCodeDto sendVerifyCodeDto);

        Task<TwoStepVerificationSignInResponseDto> VerifyCode(VerifyCodeDto model);

        Task ForgetPassword(ForgetPasswordViewModel model);

        Task ResetPassword(ResetPasswordViewModel model);

        Task<UserViewModel> ChangeLockOutEnable(ChangeLockOutStatus model);

        Task<UserViewModel> LockUserAccount(string userId);

        Task<UserViewModel> UnLockUserAccount(string userId);

        Task<UserViewModel> ChangeActiveStatus(ChangeActiveStatus model, CancellationToken cancellationToken);

        Task<UserViewModel> ResetPasswordByAdmin(ResetPasswordByAdmin model);

        Task<string> SendSms();

        Task<UserViewModel> ChangeTwoFactorStatus(string userId, CancellationToken cancellationToken);

        Task<UserViewModel> ChangeEmailConfirmedStatus(string userId, CancellationToken cancellationToken);

        Task<UserViewModel> ChangePhoneNumberConfirmedStatus(string userId, CancellationToken cancellationToken);

        Task<UserViewModel> ChangePasswordAsync(string userId,ChangePasswordDto model);
    }
}