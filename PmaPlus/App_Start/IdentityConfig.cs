﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using PmaPlus.Data;
using PmaPlus.Data.Infrastructure;
using PmaPlus.Data.Repository;
using PmaPlus.Data.Repository.Iterfaces;
using PmaPlus.Model;
using PmaPlus.Model.Models;

namespace PmaPlus
{
    public class MyUserStore : IUserStore<User, int>, 
        IUserPasswordStore<User, int>, 
        IUserLockoutStore<User, int>, 
        IUserTwoFactorStore<User, int>, 
        IUserRoleStore<User, int>,
        IUserEmailStore<User,int>
    {
        private IUserRepository _userRepository;

        public MyUserStore()
        {
            _userRepository = new UserRepository(new DatabaseFactory());
        }
        public MyUserStore(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Dispose()
        {

        }

        public Task CreateAsync(User user)
        {
            user.CreateAt = DateTime.Now;
            user.UpdateAt = DateTime.Now;
            user.LoggedAt = DateTime.Now;
            _userRepository.Add(user);

            return Task.FromResult<object>(null);
        }

        public Task UpdateAsync(User user)
        {
            user.UpdateAt = DateTime.Now;
            _userRepository.Update(user);

            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(User user)
        {
            _userRepository.Delete(user);
            return Task.FromResult<object>(null);
        }

        public Task<User> FindByIdAsync(int userId)
        {

            return Task.FromResult<User>(_userRepository.GetById(userId));
        }

        public Task<User> FindByNameAsync(string userName)
        {
            return Task.FromResult<User>(_userRepository.Get(u => u.UserName == userName));
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.Password = passwordHash;
            return Task.FromResult<object>(null);
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            user.LoggedAt = DateTime.Now;
            _userRepository.Update(user);
            return Task.FromResult<string>(_userRepository.Get(u => u.Id == user.Id).Password);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult<bool>(!_userRepository.Get(u => u.Id == user.Id).Password.IsNullOrWhiteSpace());
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            return Task.FromResult<DateTimeOffset>(new DateTimeOffset(new DateTime(1, 1, 1)));
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            return Task.FromResult<object>(null);
        }

        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            return Task.FromResult<int>(0);
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            return Task.FromResult<object>(null);
        }

        public Task<int> GetAccessFailedCountAsync(User user)
        {
            return Task.FromResult<int>(0);
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            return Task.FromResult(false);
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            return Task.FromResult<object>(null);
        }

        public Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            return Task.FromResult<object>(null);
        }

        public Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            return Task.FromResult(false);
        }

        public Task AddToRoleAsync(User user, string roleName)
        {
            Role role;
            Enum.TryParse(roleName, true, out role);
            _userRepository.GetById(user.Id).Role = role;
            return Task.FromResult<object>(null);
        }

        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            return Task.FromResult<object>(null);
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            string role = _userRepository.GetById(user.Id).Role.ToString();
            return Task.FromResult<IList<string>>(new List<string>() { role.ToString() });
        }

        public Task<bool> IsInRoleAsync(User user, string roleName)
        {
            Role role;
            Enum.TryParse(roleName, true, out role);
            if (_userRepository.GetById(user.Id).Role == role)
            {
                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
            }

        }

        public Task SetEmailAsync(User user, string email)
        {
            User _user = _userRepository.GetById(user.Id);
            if (_user.Id != 0)
            {
                _user.Email = email;
            }
            return Task.FromResult<object>(null);
        }

        public Task<string> GetEmailAsync(User user)
        {
            return Task.FromResult<string>(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user)
        {
            return Task.FromResult(true);
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            return Task.FromResult<object>(null);
        }

        public Task<User> FindByEmailAsync(string email)
        {
            return Task.FromResult<User>(_userRepository.Get(u => u.Email == email));
        }
    }


    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }



    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<User, int>
    {
        public ApplicationUserManager(IUserStore<User, int> store, IDataProtectionProvider dataProtectionProvider)
            : base(store)
        {
            UserValidator = new UserValidator<User, int>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            //Clear password hasher
            PasswordHasher = new ClearPassword();

            // Configure user lockout defaults
            UserLockoutEnabledByDefault = false;
            //DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<User, int>
            {
                MessageFormat = "Your security code is {0}"
            });
            RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<User, int>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            EmailService = new EmailService();


            UserTokenProvider = new DataProtectorTokenProvider<User, int>(dataProtectionProvider.Create("ASP.NET Identity"));

        }

       
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<User, int>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options,
            IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }


    public class ClearPassword : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return password;
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (hashedPassword.Equals(providedPassword))
            {
                return PasswordVerificationResult.Success;
            }
            else
            {
                return PasswordVerificationResult.Failed;
            }
        }
    }
}