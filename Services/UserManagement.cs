using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Task4.Models;
using Task4.Models.Enums;
using Task4.Services.Interfaces;

namespace Task4.Services;

public class UserManagement : IUserManagement
{
    private readonly UserManager<User> _userManager;

    public UserManagement(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<User>> GetUsers()
    {
        return await _userManager.Users.ToListAsync();
    }

    public async Task HandleUserManageActions(UserManageActions action, List<string> emails)
    {
        if (action == UserManageActions.Delete)
            await DeleteUsers(emails);

        if (action == UserManageActions.Block)
            await BlockUsers(emails);

        if (action == UserManageActions.Unblock)
            await UnblockUsers(emails);
    }

    public async Task<bool> IsUserBlocked(string? email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user != null && user.IsBlocked)
            return true;

        return false;
    }

    private async Task UnblockUsers(List<string> emails)
    {
        foreach (var email in emails)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                user.IsBlocked = false;

                await _userManager.UpdateAsync(user);
            }
        }
    }

    private async Task BlockUsers(List<string> emails)
    {
        foreach (var email in emails)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                user.IsBlocked = true;

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