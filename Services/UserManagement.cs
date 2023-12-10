using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Task4.Models;
using Task4.Models.Enums;
using Task4.Services.Interfaces;

namespace Task4.Services;

public class UserManagement : IUserManagement
{
    private readonly UserManager<User> _userManager;

    public UserManagement(UserManager<User> userManager) => _userManager = userManager;

    public async Task<List<User>> GetUsersAsync() => await _userManager.Users.ToListAsync();
    
    public async Task HandleUserManageActionsAsync(UserManageActions action, List<string> emails)
    {
        if (action == UserManageActions.Delete)
            await DeleteUsers(emails);
        if (action == UserManageActions.Block)
            await ProcessUsersAsync(emails, true);
        if (action == UserManageActions.Unblock)
            await ProcessUsersAsync(emails, false);
    }

    public async Task<bool> IsUserBlockedOrNotExistAsync(string? email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user == null || user.IsBlocked;
    }

    private async Task ProcessUsersAsync(List<string> emails, bool isBlocked)
    {
        foreach (var email in emails)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                user.IsBlocked = isBlocked;
                await _userManager.UpdateAsync(user);
            }
        }
    }

    private async Task DeleteUsers(List<string> emails)
    {
        foreach (var email in emails)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
                await _userManager.DeleteAsync(user);
        }
    }
}