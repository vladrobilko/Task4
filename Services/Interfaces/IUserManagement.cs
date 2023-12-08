using Task4.Models;
using Task4.Models.Enums;

namespace Task4.Services.Interfaces;

public interface IUserManagement
{
    Task<List<User>> GetUsers();

    Task HandleUserManageActions(UserManageActions action, List<string> emails);

    Task<bool> IsUserBlocked(string? email);
}