using Common;
using Common.Exceptions;
using Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Token;

namespace Services.Services
{
    public class JwtService : IJwtService
    {
        private readonly SiteSettings _siteSetting;
        private readonly SignInManager<User> signInManager;
        private readonly IUserService _userService;

        public JwtService(
            IOptionsSnapshot<SiteSettings> settings,
            SignInManager<User> signInManager,
            IUserService userService)
        {
            _siteSetting = settings.Value;
            this.signInManager = signInManager;
            _userService = userService;
        }

        public async Task<UserToken> GenerateAsync(User user)
        {
            var secretKey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.SecretKey);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

            var encryptionkey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.Encryptkey);
            var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);
            var expirationDate = DateTime.Now.AddMinutes(_siteSetting.JwtSettings.ExpirationMinutes);
            var claims = await _getClaimsAsync(user);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _siteSetting.JwtSettings.Issuer,
                Audience = _siteSetting.JwtSettings.Audience,
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddMinutes(_siteSetting.JwtSettings.NotBeforeMinutes),
                Expires = expirationDate,
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials,
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(descriptor);

            var jwt = tokenHandler.WriteToken(securityToken);

            return new UserToken() { Token = jwt, Expiration = expirationDate };
        }

        private async Task<IEnumerable<Claim>> _getClaimsAsync(User user)
        {
            #region SampleWith Identity

            //var claims = new List<Claim>()
            //{
            //    new Claim(ClaimTypes.Name, user.UserName),
            //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            //    new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
            //    new Claim("SecurityStampClaimTypes", user.SecurityStamp),
            //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            //};

            //var roles = await _userService.GetAllUserRolesAsync(user.Id.ToString());
            //foreach (var role in roles)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, role));
            //}

            #endregion SampleWith Identity

            var findUser = await _userService.GetUserAsync(user.Id.ToString());
            if (findUser == null)
                throw new NotFoundException($"خطای یافتن کاربر , برای شناسه ${user.Id} کاربری یافت نشد.");

            var result = await signInManager.ClaimsFactory.CreateAsync(user);
            var list = new List<Claim>(result.Claims);
            list.Add(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber));
            list.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            return list;
        }
    }
}