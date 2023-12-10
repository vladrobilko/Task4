using Task4.Models;
using Task4.Models.Enums;

namespace Task4.Services.Interfaces;

public interface IUserManagement
{
    Task<List<User>> GetUsersAsync();

    Task HandleUserManageActionsAsync(UserManageActions action, List<string> emails);

    Task<bool> IsUserBlockedOrNotExistAsync(string? email);
}