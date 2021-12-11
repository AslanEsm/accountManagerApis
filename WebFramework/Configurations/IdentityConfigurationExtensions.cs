using System;
using Common;
using Data;
using Entities;
using Entities.Role;
using Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace WebFramework.Configurations
{
    public static class IdentityConfigurationExtensions
    {
        public static void AddCustomIdentity(this IServiceCollection services, IdentitySettings settings)
        {
            services.AddIdentity<User, Role>(identityOptions =>
                {
                    identityOptions.Password.RequireDigit = settings.PasswordRequiredDigit;
                    identityOptions.Password.RequiredLength = settings.PasswordRequiredLength;
                    identityOptions.Password.RequireNonAlphanumeric = settings.PasswordRequireNonAlphanumeric;
                    identityOptions.Password.RequireLowercase = settings.PasswordRequireLowercase;
                    identityOptions.Password.RequireUppercase = settings.PasswordRequireUppercase;

                    identityOptions.User.RequireUniqueEmail = settings.RequireUniqueEmail;

                    identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(settings.DefaultLockoutTimeSpan);
                    identityOptions.Lockout.MaxFailedAccessAttempts = settings.MaxFailedAccessAttempts;

                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<ApplicationIdentityErrorDescriber>();
        }
    }
}