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

        public async Task<bool> RegisterAsync(RegisterUserViewModel registerModel)
        {
            if (!IsRegisterModelValid(registerModel)) return false;
            var user = CreateUser(registerModel);
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (result.Succeeded) await _signInManager.SignInAsync(user, isPersistent: false);
            return result.Succeeded;
        }

        public async Task<bool> LoginAsync(LoginUserViewModel loginModel)
        {
            if (!IsLoginModelValid(loginModel)) return false;
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user?.IsBlocked == true) return false;
            if (user?.IsBlocked == false && (await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false)).Succeeded)
                await UpdateUserAsync(user);
            return user?.IsBlocked == false;
        }

        private async Task UpdateUserAsync(User user)
        {
            user.LastLoginDate = DateTime.Now;
            await _userManager.UpdateAsync(user);
        }

        public async Task LogoutAsync() => await _signInManager.SignOutAsync();

        private User CreateUser(RegisterUserViewModel registerModel) =>
            new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = registerModel.Email,
                Name = registerModel.Name,
                Email = registerModel.Email,
                RegistrationDate = DateTime.Now,
                LastLoginDate = DateTime.Now,
                IsBlocked = false
            };

        private bool IsRegisterModelValid(RegisterUserViewModel registerModel) =>
            !string.IsNullOrEmpty(registerModel.Email) &&
            !string.IsNullOrEmpty(registerModel.Name) &&
            !string.IsNullOrEmpty(registerModel.Password);

        private bool IsLoginModelValid(LoginUserViewModel loginModel) =>
             !string.IsNullOrEmpty(loginModel.Email) &&
             !string.IsNullOrEmpty(loginModel.Password);
    }
}
