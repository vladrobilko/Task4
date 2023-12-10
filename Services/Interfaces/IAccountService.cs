using Task4.Models;

namespace Task4.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> RegisterAsync(RegisterUserViewModel registerModel);

        Task<bool> LoginAsync(LoginUserViewModel loginModel);

        Task LogoutAsync();
    }
}