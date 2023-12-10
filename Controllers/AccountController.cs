using Microsoft.AspNetCore.Mvc;
using Task4.Models;
using Task4.Services.Interfaces;

namespace Task4.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService) => _accountService = accountService;

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel registerModel)
        {
            if (await _accountService.RegisterAsync(registerModel))
                return RedirectToAction("GetUsers", "UserManagement", new { userEmail = registerModel.Email });
            return RedirectToAction("Register");
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel loginModel)
        {
            if (await _accountService.LoginAsync(loginModel))
                return RedirectToAction("GetUsers", "UserManagement", new { userEmail = loginModel.Email });
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction("Login");
        }
    }
}