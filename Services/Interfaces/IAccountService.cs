using Task4.Models;

namespace Task4.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> Register(RegisterUserViewModel registerModel);

        Task<bool> Login(LoginUserViewModel loginModel);

        Task Logout();
    }
}
