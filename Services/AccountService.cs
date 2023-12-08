using Microsoft.AspNetCore.Identity;
using Task4.Models;
using Task4.Services.Interfaces;

namespace Task4.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }

        public async Task<bool> Register(RegisterUserViewModel registerModel)
        {
            if (!IsRegisterModelValid(registerModel)) return false;

            var user = CreateUser(registerModel);

            var result = await _userManager.CreateAsync(user, registerModel.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);

                return true;
            }

            return false;
        }

        public async Task<bool> Login(LoginUserViewModel loginModel)
        {
            if (!IsLoginModelValid(loginModel)) return false;

            var user = await _userManager.FindByEmailAsync(loginModel.Email);

            if (user == null || user.IsBlocked) return false;

            var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password,
                isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                user.LastLoginDate = DateTime.Now;
                await _userManager.UpdateAsync(user);

                return true;
            }

            return false;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        private static User CreateUser(RegisterUserViewModel registerModel)
        {
            return new User()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = registerModel.Email,
                Name = registerModel.Name,
                Email = registerModel.Email,
                RegistrationDate = DateTime.Now,
                LastLoginDate = DateTime.Now,
                IsBlocked = false
            };
        }

        private bool IsRegisterModelValid(RegisterUserViewModel registerModel)
        {
            return !string.IsNullOrEmpty(registerModel.Email)
                   && !string.IsNullOrEmpty(registerModel.Name)
                   && !string.IsNullOrEmpty(registerModel.Password);
        }

        private bool IsLoginModelValid(LoginUserViewModel loginModel)
        {
            return !string.IsNullOrEmpty(loginModel.Email)
                   && !string.IsNullOrEmpty(loginModel.Password);
        }
    }
}
