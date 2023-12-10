using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Task4.Helpers;
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
            var result = await CreateAndSignInUserAsync(registerModel, CreateUser(registerModel));
            return result.Succeeded;
        }

        public async Task<bool> LoginAsync(LoginUserViewModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null)
            {
                loginModel.SetErrorMessage(null, user);
                return false;
            }
            var signInResult = await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);
            if (!user.IsBlocked && signInResult.Succeeded) await UpdateUserAsync(user);
            else loginModel.SetErrorMessage(signInResult, user);
            return user?.IsBlocked == false && signInResult.Succeeded;
        }

        public async Task LogoutAsync() => await _signInManager.SignOutAsync();

        private async Task<IdentityResult> CreateAndSignInUserAsync(RegisterUserViewModel registerModel, User user)
        {
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (result.Succeeded)
                await _signInManager.SignInAsync(user, isPersistent: false);
            else
                registerModel.ErrorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            return result;
        }

        private async Task UpdateUserAsync(User user)
        {
            user.LastLoginDate = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
        }

        private User CreateUser(RegisterUserViewModel registerModel) =>
            new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = registerModel.Email,
                Name = registerModel.Name,
                Email = registerModel.Email,
                RegistrationDate = DateTime.UtcNow,
                LastLoginDate = DateTime.UtcNow,
                IsBlocked = false
            };
    }
}