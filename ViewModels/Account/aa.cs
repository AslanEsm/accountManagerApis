//using AutoMapper;
//using common.Utilities;
//using Common;
//using Common.Exceptions;
//using Common.Utilities;
//using Entities.User;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Options;
//using Services.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Web;
//using Microsoft.Extensions.Configuration;
//using ViewModels.Account;
//using ViewModels.Results;
//using ViewModels.Role;
//using ViewModels.User;

//namespace Services.Services
//{
//    public class AccountService : IAccountService
//    {
//        #region Constructor

//        private readonly IUserService _userService;
//        private readonly UserManager<User> _userManager;
//        private readonly IRoleService _roleService;
//        private readonly IEmailService _emailService;
//        private readonly IViewRenderService _viewRenderService;
//        private readonly SiteSettings _siteSetting;
//        private readonly IJwtService _jwtService;
//        private readonly IMapper _mapper;
//        private IConfigurationProvider _configuration;
//        private readonly SignInManager<User> _signInManager;
//        private readonly ISmsSender _smsSender;

//        public AccountService(
//            IUserService userService,
//            UserManager<User> userManager,
//            IRoleService roleService,
//            IEmailService emailService,
//            IViewRenderService viewRenderService,
//            IOptionsSnapshot<SiteSettings> settings,
//            IJwtService jwtService,
//            IMapper mapper,
//            IConfigurationProvider configuration,
//            SignInManager<User> signInManager,
//            ISmsSender smsSender)
//        {
//            _userService = userService;
//            _userManager = userManager;
//            _roleService = roleService;
//            _emailService = emailService;
//            _viewRenderService = viewRenderService;
//            _jwtService = jwtService;
//            _siteSetting = settings.Value;
//            _mapper = mapper;
//            _configuration = configuration;
//            _signInManager = signInManager;
//            _smsSender = smsSender;
//        }

//        #endregion Constructor

//        #region Methods

//        #region Register

//        public async Task<RegisterResult> Register(RegisterDto registerModel)
//        {
//            if (await _userService.IsUserExistByUserName(registerModel.UserName))
//                return new RegisterResult(Result.DuplicateUserName);

//            if (await _userService.IsUserExistByEmail(registerModel.Email))
//                return new RegisterResult(Result.DuplicateEmail);

//            var user = new User()
//            {
//                Email = registerModel.Email.SanitizeText(),
//                UserName = registerModel.UserName.SanitizeText(),
//                PhoneNumber = registerModel.PhoneNumber.SanitizeText(),
//                IsActive = true
//            };

//            var result = await _userManager.CreateAsync(user, registerModel.Password);

//            if (!result.Succeeded)
//            {
//                var errorList = result.Errors.ToList();
//                return new RegisterResult(Result.ServerError, errorList);
//            }

//            var role = await _roleService.GetRoleByName("user");
//            if (role == null)
//            {
//                var newRole = new CreateRole()
//                {
//                    Name = "user",
//                    Description = "این سطح دسترسی برای کاربران عادی سایت می باشد"
//                };

//                var addRoleResult = await _roleService.CreateAsync(newRole);
//            }

//            var res = await _userService.AddRoleToUser(user.Id, "user");

//            if (!res)
//            {
//                await _userManager.DeleteAsync(user);
//                return new RegisterResult(Result.AddToRoleOperation);
//            }

//            //Sending activation Email
//            //By using this encode utility method, the encoded code that is URL safe for the activation link becomes
//            var userId = user.Id.ToString();
//            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
//            var baseUrl = _siteSetting.BaseUrl.Url;
//            var encodedCode = HttpUtility.UrlEncode(code);
//            var callbackUrl = $"{baseUrl}/api/Account/ActivateAccount?userId={userId}&code={encodedCode}";

//            #region OldCode

//            //var callbackUrl =
//            //    string.Format(
//            //        "https://localhost:44315/api/Account/ActivateAccount?userId={0}&code={1}",
//            //        HttpUtility.UrlEncode(user.Id.ToString()), HttpUtility.UrlEncode(code));

//            #endregion OldCode

//            var registerWithEmailConfirmCode = new RegisterWithEmailConfirmCodeDto()
//            {
//                CallBackUrl = callbackUrl,
//                User = user,
//            };
//            var body = await _viewRenderService.RenderToStringAsync("Emails/ActivateAccount",
//                registerWithEmailConfirmCode);
//            await _emailService.SendEmailAsync(registerModel.Email, "ایمیل فعال سازی", body);

//            return new RegisterResult(Result.Success);
//        }

//        public async Task<RegisterResult> RegisterByAdmin(RegisterByAdminDto registerModel)
//        {
//            if (registerModel.UserRoles == null)
//                throw new LogicException("یک نقش برای کاربر انتخاب کنید");

//            if (await _userService.IsUserExistByUserName(registerModel.UserName))
//                return new RegisterResult(Result.DuplicateUserName);

//            if (await _userService.IsUserExistByEmail(registerModel.Email))
//                return new RegisterResult(Result.DuplicateEmail);

//            var user = new User()
//            {
//                Email = registerModel.Email.SanitizeText(),
//                UserName = registerModel.UserName.SanitizeText(),
//                FirstName = registerModel.FirstName.SanitizeText(),
//                LastName = registerModel.LastName.SanitizeText(),
//                PhoneNumber = registerModel.PhoneNumber.SanitizeText(),
//                BirthDate = registerModel.BirthDate.ToMiladiDateTime(),
//                IsActive = true,
//                EmailConfirmed = true,
//                TwoFactorEnabled = registerModel.TwoFactorEnabled
//            };

//            var result = await _userManager.CreateAsync(user, registerModel.Password);

//            if (!result.Succeeded)
//            {
//                var errorList = result.Errors.ToList();
//                return new RegisterResult(Result.ServerError, errorList);
//            }

//            var res = await _userService.AddRolesToUserAsync(user.Id, registerModel.UserRoles);

//            if (!res)
//            {
//                await _userManager.DeleteAsync(user);
//                return new RegisterResult(Result.AddToRoleOperation);
//            }

//            return new RegisterResult(Result.Success);
//        }

//        #endregion Register

//        #region Login

//        public async Task<SignInResponseDto> SignIn(SignInViewModel signInModel)
//        {
//            //get user
//            var user = await _userManager.FindByNameAsync(signInModel.UserName);
//            if (user == null)
//                throw new BadRequestException($"کاربری با نام کاربری {signInModel.UserName} وجود ندارد.");

//            //check user is active or not
//            if (!user.IsActive)
//                throw new BadRequestException("حساب کاربری شما توسط امین سایت بسته شده است");

//            //check pass
//            var result =
//                await _signInManager.PasswordSignInAsync(user, signInModel.Password, signInModel.RememberMe, true);

//            if (result.IsLockedOut)
//                throw new BadRequestException("حساب کاربری شما به مدت 20 دقیقه به دلیل تلاش های ناموفق قفل شده است");

//            if (!result.Succeeded)
//                throw new BadRequestException("نام کاربری و یا رمز عبور اشتباه است");

//            if (!await _userManager.IsEmailConfirmedAsync(user))
//                throw new BadRequestException("ایمیل شما تایید نشده است");

//            #region CheckPass-other way

//            //if (!await _userManager.CheckPasswordAsync(user, signInModel.Password))
//            //    throw new BadRequestException("نام کاربری و یا رمز عبور اشتباه است");

//            #endregion CheckPass-other way

//            if (await _userManager.GetTwoFactorEnabledAsync(user))
//                return await _generateOTPFor2StepVerification(user, signInModel.RememberMe);

//            //get token
//            var userToken = await _jwtService.GenerateAsync(user);

//            //building return model

//            var mappedUser = _mapper.Map<User, UserViewModel>(user);
//            var returnModel = new SignInResponseDto()
//            {
//                User = mappedUser,
//                Token = userToken,
//                RememberMe = signInModel.RememberMe,
//                Providers = null,
//                Is2StepVerificationRequired = false
//            };

//            return returnModel;
//        }

//        private async Task<SignInResponseDto> _generateOTPFor2StepVerification(User user, bool rememberMe)
//        {
//            //get providers
//            var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);
//            var mappedUser = _mapper.Map<User, UserViewModel>(user);
//            var checkProvidersResponse = new SignInResponseDto()
//            {
//                User = mappedUser,
//                Providers = providers,
//                RememberMe = rememberMe,
//                Token = null,
//                Is2StepVerificationRequired = true
//            };

//            return checkProvidersResponse;
//        }

//        public async Task SendVerifyCode(SendVerifyCodeDto sendVerifyCodeDto)
//        {
//            //get user
//            var user = await _userService.GetUserAsync(sendVerifyCodeDto.UserId);
//            //generate token
//            var token = await _userManager.GenerateTwoFactorTokenAsync(user, sendVerifyCodeDto.SelectedProvider);
//            if (string.IsNullOrWhiteSpace(token))
//                throw new AppException("خطای رخ داده است,لطفا مجددا تلاش کنید");
//            //sending verify email
//            if (sendVerifyCodeDto.SelectedProvider == "Email")
//            {
//                var emailModel = new TwoStepVerificationEmailDto() { User = user, Token = token };
//                var body = await _viewRenderService.RenderToStringAsync("2StepVerification/2StepVerification",
//                    emailModel);
//                await _emailService.SendEmailAsync(user.Email, "کد اعتبار سنجی", body);
//            }

//            //sending verify sms
//            if (sendVerifyCodeDto.SelectedProvider == "Phone")
//            {
//                var responseSms = await _smsSender.SendAuthSmsAsync(token, user.PhoneNumber);
//                if (responseSms == "Fail")
//                    throw new AppException("خطای ارسال پیامک");
//            }
//        }

//        public async Task<TwoStepVerificationSignInResponseDto> VerifyCode(VerifyCodeDto model)
//        {
//            //get user
//            var user = await _userService.GetUserAsync(model.UserId);
//            //validate token
//            var validateVerification = await _userManager.VerifyTwoFactorTokenAsync(user, model.Provider, model.Code);
//            if (!validateVerification)
//                throw new BadRequestException("خطای اعتبارسنجی");

//            //get token
//            var userToken = await _jwtService.GenerateAsync(user);
//            //building return model
//            var mappedUser = _mapper.Map<User, UserViewModel>(user);
//            var returnModel = new TwoStepVerificationSignInResponseDto()
//            {
//                User = mappedUser,
//                Token = userToken
//            };

//            return returnModel;
//        }

//        #endregion Login

//        public async Task<bool> ConfirmEmail(string userId, string code)
//        {
//            if (userId == null || code == null)
//                throw new NotFoundException("خطای تایید ایمیل");

//            var user = await _userService.GetUserAsync(userId);
//            var result = await _userManager.ConfirmEmailAsync(user, code);

//            if (!result.Succeeded)
//            {
//                List<IdentityError> errorList = result.Errors.ToList();
//                var errors = string.Join(", ", errorList.Select(e => e.Description));
//                throw new AppException($"{errors}");
//            }

//            return result.Succeeded;
//        }

//        public async Task ForgetPassword(ForgetPasswordViewModel model)
//        {
//            //check input
//            if (model.Email == null)
//                throw new NotFoundException("خطای ایمیل ورودی");

//            //check user
//            var user = await _userManager.FindByEmailAsync(model.Email);
//            if (user == null)
//                throw new NotFoundException($"کاربری با ایمیل {model.Email} یافت نشد.");

//            //check email confirmed
//            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
//            if (!isEmailConfirmed)
//                throw new BadRequestException("لطفا ایمیل خود را تایید کنید");

//            //build callback url
//            var email = user.Email;
//            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
//            var baseUrl = _siteSetting.BaseUrl.Url;
//            var encodedCode = HttpUtility.UrlEncode(code);
//            var callbackUrl = $"{baseUrl}/api/Account/ResetPassword?email={email}&code={encodedCode}";

//            //create model for view
//            var forgetPasswordWithEmailConfirmCode = new ForgetPasswordWithEmailConfirmCode()
//            {
//                CallBackUrl = callbackUrl,
//                User = user,
//            };

//            //sending email
//            var body = await _viewRenderService.RenderToStringAsync("ResetPassword/ResetPassword",
//                forgetPasswordWithEmailConfirmCode);
//            await _emailService.SendEmailAsync(model.Email, "ایمیل بازیابی رمز عبور", body);
//        }

//        public async Task ResetPassword(ResetPasswordViewModel model)
//        {
//            //check user exist
//            var user = await _userManager.FindByEmailAsync(model.Email);
//            if (user == null)
//                throw new NotFoundException($"کاربری با ایمیل {model.Email} یافت نشد.");

//            //compare passwords
//            if (model.Password != model.ConfirmPassword)
//                throw new BadRequestException("عدم تطابق رمز عبور");

//            //decode token
//            var token = HttpUtility.UrlDecode(model.Code);

//            //reset pass
//            var result = await _userManager.ResetPasswordAsync(user, token, model.Password);

//            if (!result.Succeeded)
//            {
//                List<IdentityError> errorList = result.Errors.ToList();
//                var errors = string.Join(", ", errorList.Select(e => e.Description));
//                throw new AppException($"{errors}");
//            }
//        }

//        public async Task<UserViewModel> ChangeLockOutEnable(ChangeLockOutStatus model)
//        {
//            //get user
//            var user = await _userService.GetUserAsync(model.UserId);
//            //change lockout status
//            var result = await _userManager.SetLockoutEnabledAsync(user, model.Status);
//            //map user to userViewModel
//            var userDto = _mapper.Map<User, UserViewModel>(user);

//            return userDto;
//        }

//        public async Task<UserViewModel> LockUserAccount(string userId)
//        {
//            //get user
//            var user = await _userService.GetUserAsync(userId);
//            //get lock time span
//            var lockTimeSpan = _siteSetting.IdentitySettings.DefaultLockoutTimeSpan;
//            //lock user account
//            var res = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(lockTimeSpan));
//            if (!res.Succeeded)
//            {
//                List<IdentityError> errorList = res.Errors.ToList();
//                var errors = string.Join(", ", errorList.Select(e => e.Description));
//                throw new AppException($"{errors}");
//            }

//            //map user
//            var userDto = _mapper.Map<User, UserViewModel>(user);

//            return userDto;
//        }

//        public async Task<UserViewModel> UnLockUserAccount(string userId)
//        {
//            //get user
//            var user = await _userService.GetUserAsync(userId);
//            //unLock user account by passing null as a second arg
//            var res = await _userManager.SetLockoutEndDateAsync(user, null);
//            if (!res.Succeeded)
//            {
//                List<IdentityError> errorList = res.Errors.ToList();
//                var errors = string.Join(", ", errorList.Select(e => e.Description));
//                throw new AppException($"{errors}");
//            }

//            //map user
//            var userDto = _mapper.Map<User, UserViewModel>(user);

//            return userDto;
//        }

//        public async Task<UserViewModel> ChangeActiveStatus(ChangeActiveStatus model,
//            CancellationToken cancellationToken)
//        {
//            //get user
//            var user = await _userService.GetUserAsync(model.UserId);
//            //update active status
//            user.IsActive = model.Status;
//            //map to editUser model for update
//            var editUser = _mapper.Map<User, EditUser>(user);
//            //update user
//            var res = await _userService.UpdateUserAsync(editUser, cancellationToken);
//            return res;
//        }

//        public async Task<UserViewModel> ResetPasswordByAdmin(ResetPasswordByAdmin model)
//        {
//            //get user
//            var user = await _userService.GetUserAsync(model.Id);

//            //remove user password
//            var res = await _userManager.RemovePasswordAsync(user);
//            if (!res.Succeeded)
//            {
//                List<IdentityError> errorList = res.Errors.ToList();
//                var errors = string.Join(", ", errorList.Select(e => e.Description));
//                throw new AppException($"{errors}");
//            }

//            //add password to user
//            res = await _userManager.AddPasswordAsync(user, model.NewPassword);
//            if (!res.Succeeded)
//            {
//                List<IdentityError> errorList = res.Errors.ToList();
//                var errors = string.Join(", ", errorList.Select(e => e.Description));
//                throw new AppException($"{errors}");
//            }

//            //map user to userViewModel
//            var userDto = _mapper.Map<User, UserViewModel>(user);
//            return userDto;
//        }

//        public async Task<string> SendSms()
//        {
//            string status = await _smsSender.SendAuthSmsAsync("46535", "09117128622");
//            if (status == "Fail")
//                throw new AppException("خطای ارسال پیامک");

//            return "ارسال پیامک با موفقیت انجام شد";
//        }

//        public async Task<UserViewModel> ChangeTwoFactorStatus(string userId, CancellationToken cancellationToken)
//        {
//            //get user
//            var user = await _userService.GetUserAsync(userId);
//            //change TwoFactor status(toggle mode)
//            if (user.TwoFactorEnabled)
//                user.TwoFactorEnabled = false;
//            else
//                user.TwoFactorEnabled = true;

//            //map to editUser model for update
//            var editUser = _mapper.Map<User, EditUser>(user);
//            //update user
//            var res = await _userService.UpdateUserAsync(editUser, cancellationToken);
//            return res;
//        }

//        public async Task<UserViewModel> ChangeEmailConfirmedStatus(string userId, CancellationToken cancellationToken)
//        {
//            //get user
//            var user = await _userService.GetUserAsync(userId);
//            //change EmailConfirmed status(toggle mode)
//            if (user.EmailConfirmed)
//                user.EmailConfirmed = false;
//            else
//                user.EmailConfirmed = true;

//            //map to editUser model for update
//            var editUser = _mapper.Map<User, EditUser>(user);
//            //update user
//            var res = await _userService.UpdateUserAsync(editUser, cancellationToken);
//            return res;
//        }

//        public async Task<UserViewModel> ChangePhoneNumberConfirmedStatus(string userId,
//            CancellationToken cancellationToken)
//        {
//            //get user
//            var user = await _userService.GetUserAsync(userId);
//            //change PhoneNumberConfirmed status(toggle mode)
//            if (user.PhoneNumberConfirmed)
//                user.PhoneNumberConfirmed = false;
//            else
//                user.PhoneNumberConfirmed = true;

//            //map to editUser model for update
//            var editUser = _mapper.Map<User, EditUser>(user);
//            //update user
//            var res = await _userService.UpdateUserAsync(editUser, cancellationToken);
//            return res;
//        }

//        #endregion Methods
//    }

//}